using System.Security.Claims;
using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.Models;
using CinemaPlus.CinemaWebApp.Services;
using CinemaPlus.CinemaWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Controllers;

public class CheckoutController(
    ApplicationDbContext dbContext,
    IWebHostEnvironment environment,
    ICinemaTicketService ticketService) : Controller
{
    [HttpGet("checkout")]
    public async Task<IActionResult> Index(int sessionId, [FromQuery] int[] seatIds)
    {
        var selectedSeatIds = NormalizeSeatIds(seatIds);
        if (sessionId <= 0 || selectedSeatIds.Count == 0)
        {
            return RedirectToAction(nameof(SessionsController.Index), "Sessions");
        }

        var checkoutData = await LoadCheckoutDataAsync(sessionId);
        if (checkoutData is null)
        {
            return NotFound();
        }

        var hallSeatIds = checkoutData.HallSeats.Select(seat => seat.Id).ToHashSet();
        var bookedSeatIds = checkoutData.BookedSeatIds.ToHashSet();

        if (selectedSeatIds.Any(seatId => !hallSeatIds.Contains(seatId)) || selectedSeatIds.Any(bookedSeatIds.Contains))
        {
            return RedirectToAction(nameof(SessionsController.Seats), "Sessions", new { sessionId });
        }

        var viewModel = await BuildCheckoutViewModelAsync(checkoutData, selectedSeatIds, null);
        return View(viewModel);
    }

    [HttpPost("checkout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(CheckoutSelectionViewModel model)
    {
        var selectedSeatIds = NormalizeSeatIds(model.SelectedSeatIds);
        var checkoutData = await LoadCheckoutDataAsync(model.SessionId);
        if (checkoutData is null)
        {
            return NotFound();
        }

        if (selectedSeatIds.Count == 0)
        {
            return RedirectToAction(nameof(SessionsController.Seats), "Sessions", new { sessionId = model.SessionId });
        }

        var useApplePay = string.Equals(model.PaymentMethod, "ApplePay", StringComparison.OrdinalIgnoreCase);
        if (useApplePay)
        {
            ModelState.Remove(nameof(model.CardNumber));
            ModelState.Remove(nameof(model.CardExpiry));
            ModelState.Remove(nameof(model.CardCvc));
        }

        var hallSeatIds = checkoutData.HallSeats.Select(seat => seat.Id).ToHashSet();
        var bookedSeatIds = checkoutData.BookedSeatIds.ToHashSet();
        var validSeatIds = selectedSeatIds
            .Where(seatId => hallSeatIds.Contains(seatId) && !bookedSeatIds.Contains(seatId))
            .ToList();

        if (validSeatIds.Count != selectedSeatIds.Count)
        {
            ModelState.AddModelError(string.Empty, "Деякі обрані місця вже недоступні. Поверніться до схеми залу та оновіть вибір.");
        }

        if (!ModelState.IsValid)
        {
            var invalidViewModel = await BuildCheckoutViewModelAsync(checkoutData, validSeatIds, model);
            return View(invalidViewModel);
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var currentBookedSeatIds = await dbContext.BookedSeats
                .Where(item => item.SessionId == model.SessionId && selectedSeatIds.Contains(item.SeatId))
                .Select(item => item.SeatId)
                .ToListAsync();

            if (currentBookedSeatIds.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "Поки ви оформлювали замовлення, частина місць стала недоступною. Будь ласка, перевірте вибір ще раз.");
                var staleViewModel = await BuildCheckoutViewModelAsync(
                    checkoutData,
                    selectedSeatIds.Where(seatId => !currentBookedSeatIds.Contains(seatId)).ToList(),
                    model);
                return View(staleViewModel);
            }

            var booking = new Booking
            {
                UserId = GetCurrentUserId(),
                SessionId = model.SessionId,
                BookingDate = DateTime.UtcNow,
                Status = "Paid",
                TotalPrice = checkoutData.Session.Price * selectedSeatIds.Count,
                TicketCode = ticketService.GenerateTicketCode(),
                CustomerName = model.FullName.Trim(),
                CustomerEmail = model.Email.Trim().ToLowerInvariant(),
                CustomerPhone = model.Phone.Trim()
            };

            dbContext.Bookings.Add(booking);
            await dbContext.SaveChangesAsync();

            foreach (var seatId in selectedSeatIds)
            {
                dbContext.BookedSeats.Add(new BookedSeat
                {
                    BookingId = booking.Id,
                    SessionId = booking.SessionId,
                    SeatId = seatId
                });
            }

            var seatSummary = string.Join(" • ", checkoutData.HallSeats
                .Where(seat => selectedSeatIds.Contains(seat.Id))
                .OrderBy(seat => seat.RowNumber)
                .ThenBy(seat => seat.SeatNumber)
                .Select(seat => CinemaPresentationHelper.FormatSeatLabel(seat.RowNumber, seat.SeatNumber)));

            dbContext.NotificationLogs.Add(new NotificationLog
            {
                BookingId = booking.Id,
                Email = booking.CustomerEmail,
                CreatedDate = DateTime.UtcNow,
                Status = "Emulated",
                Message = $"Замовлення №{ticketService.FormatOrderNumber(booking.Id, booking.BookingDate)} успішно оплачено. Фільм: {CinemaPresentationHelper.FormatMovieTitle(checkoutData.Session.Film)}. Місця: {seatSummary}."
            });

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            if (booking.UserId.HasValue)
            {
                return RedirectToAction("Details", "Tickets", new { id = booking.Id });
            }

            return RedirectToAction("Details", "Tickets", new { id = booking.Id, ticketCode = booking.TicketCode });
        }
        catch (DbUpdateException)
        {
            await transaction.RollbackAsync();
            ModelState.AddModelError(string.Empty, "Не вдалося завершити оплату, оскільки вибрані місця вже зайняті. Оновіть замовлення та спробуйте ще раз.");
            var retryViewModel = await BuildCheckoutViewModelAsync(checkoutData, selectedSeatIds, model);
            return View(retryViewModel);
        }
    }

    private async Task<CheckoutSelectionViewModel> BuildCheckoutViewModelAsync(
        CheckoutData checkoutData,
        IReadOnlyCollection<int> selectedSeatIds,
        CheckoutSelectionViewModel? sourceModel)
    {
        var selectedSeats = checkoutData.HallSeats
            .Where(seat => selectedSeatIds.Contains(seat.Id))
            .OrderBy(seat => seat.RowNumber)
            .ThenBy(seat => seat.SeatNumber)
            .ToList();

        User? currentUser = null;
        var currentUserId = GetCurrentUserId();

        if (currentUserId.HasValue)
        {
            currentUser = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == currentUserId.Value);
        }

        return new CheckoutSelectionViewModel
        {
            SessionId = checkoutData.Session.Id,
            MovieId = checkoutData.Session.FilmId,
            PosterPath = CinemaPresentationHelper.ResolvePosterPath(checkoutData.Session.Film, environment),
            MovieTitle = CinemaPresentationHelper.FormatMovieTitle(checkoutData.Session.Film),
            Genre = checkoutData.Session.Film.Genre,
            DurationMinutes = checkoutData.Session.Film.DurationMinutes,
            AgeRestriction = checkoutData.Session.Film.AgeRestriction,
            DateText = checkoutData.Session.SessionTime.ToString("dd.MM.yyyy", CinemaPresentationHelper.UkrainianCulture),
            TimeText = checkoutData.Session.SessionTime.ToString("H:mm", CinemaPresentationHelper.UkrainianCulture),
            HallName = $"{checkoutData.Session.Hall.Name}, {checkoutData.Session.Hall.Technology}",
            PricePerSeat = checkoutData.Session.Price,
            TotalPrice = checkoutData.Session.Price * selectedSeats.Count,
            SelectedSeatIds = selectedSeats.Select(seat => seat.Id).ToList(),
            SeatLabels = selectedSeats
                .Select(seat => CinemaPresentationHelper.FormatSeatLabel(seat.RowNumber, seat.SeatNumber))
                .ToList(),
            FullName = sourceModel?.FullName ?? currentUser?.FullName ?? User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
            Email = sourceModel?.Email ?? currentUser?.Email ?? User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            Phone = sourceModel?.Phone ?? currentUser?.Phone ?? string.Empty,
            CardNumber = sourceModel?.CardNumber ?? string.Empty,
            CardExpiry = sourceModel?.CardExpiry ?? string.Empty,
            CardCvc = sourceModel?.CardCvc ?? string.Empty,
            ProfileEditorOpen = sourceModel?.ProfileEditorOpen ?? false,
            PaymentMethod = sourceModel?.PaymentMethod ?? "Card",
            IsAuthenticatedUser = currentUserId.HasValue
        };
    }

    private async Task<CheckoutData?> LoadCheckoutDataAsync(int sessionId)
    {
        var session = await dbContext.Sessions
            .AsNoTracking()
            .AsSplitQuery()
            .Include(item => item.Film)
            .Include(item => item.Hall)
                .ThenInclude(hall => hall.Seats)
            .Include(item => item.BookedSeats)
            .FirstOrDefaultAsync(item => item.Id == sessionId && item.SessionTime <= CinemaPresentationHelper.NowShowingCutoff);

        return session is null
            ? null
            : new CheckoutData(
                session,
                session.Hall.Seats
                    .OrderBy(seat => seat.RowNumber)
                    .ThenBy(seat => seat.SeatNumber)
                    .ToList(),
                session.BookedSeats.Select(item => item.SeatId).ToList());
    }

    private int? GetCurrentUserId()
    {
        return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
            ? userId
            : null;
    }

    private static IReadOnlyList<int> NormalizeSeatIds(IEnumerable<int> seatIds)
    {
        return seatIds
            .Where(id => id > 0)
            .Distinct()
            .ToList();
    }

    private sealed record CheckoutData(
        Session Session,
        IReadOnlyList<Seat> HallSeats,
        IReadOnlyList<int> BookedSeatIds);
}
