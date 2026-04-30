using System.Security.Claims;
using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.Services;
using CinemaPlus.CinemaWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Controllers;

[Authorize]
public class ProfileController(
    ApplicationDbContext dbContext,
    IWebHostEnvironment environment,
    IBonusService bonusService) : Controller
{
    [Route("profile")]
    public async Task<IActionResult> Index()
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = "/profile" });
        }

        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == currentUserId.Value);

        if (user is null)
        {
            return NotFound();
        }

        var bookings = await dbContext.Bookings
            .AsNoTracking()
            .AsSplitQuery()
            .Where(item => item.UserId == currentUserId.Value)
            .Include(item => item.Session)
                .ThenInclude(session => session.Film)
            .Include(item => item.Session)
                .ThenInclude(session => session.Hall)
            .Include(item => item.BookedSeats)
                .ThenInclude(bookedSeat => bookedSeat.Seat)
            .OrderBy(item => item.Status == "Cancelled")
            .ThenByDescending(item => item.Session.SessionTime)
            .ThenByDescending(item => item.BookingDate)
            .ToListAsync();

        var viewModel = new ProfileViewModel
        {
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone ?? "Не вказано",
            BonusLabel = bonusService.FormatBonusLabel(await bonusService.CalculateBonusAsync(currentUserId.Value)),
            StatusMessage = TempData["ProfileStatusMessage"] as string,
            Tickets = bookings.Select(booking =>
            {
                var orderedSeats = booking.BookedSeats
                    .Select(item => item.Seat)
                    .OrderBy(seat => seat.RowNumber)
                    .ThenBy(seat => seat.SeatNumber)
                    .ToList();

                return new ProfileTicketCardViewModel
                {
                    BookingId = booking.Id,
                    PosterPath = CinemaPresentationHelper.ResolvePosterPath(booking.Session.Film, environment),
                    MovieTitle = CinemaPresentationHelper.FormatMovieTitle(booking.Session.Film),
                    DateText = booking.Session.SessionTime.ToString("dd.MM.yyyy", CinemaPresentationHelper.UkrainianCulture),
                    TimeText = booking.Session.SessionTime.ToString("H:mm", CinemaPresentationHelper.UkrainianCulture),
                    StatusText = booking.Status == "Cancelled" ? "СКАСОВАНО" : "АКТИВНИЙ",
                    SeatSummaryText = orderedSeats.Count > 0
                        ? string.Join(" • ", orderedSeats.Select(seat => CinemaPresentationHelper.FormatSeatLabel(seat.RowNumber, seat.SeatNumber)))
                        : "Місця звільнено",
                    IsCancelled = booking.Status == "Cancelled",
                    CanCancel = booking.Status != "Cancelled"
                };
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost("profile/bookings/{bookingId:int}/cancel")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelBooking(int bookingId)
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = "/profile" });
        }

        var booking = await dbContext.Bookings
            .Include(item => item.BookedSeats)
            .FirstOrDefaultAsync(item => item.Id == bookingId && item.UserId == currentUserId.Value);

        if (booking is null)
        {
            return NotFound();
        }

        if (booking.Status == "Cancelled")
        {
            TempData["ProfileStatusMessage"] = "Цей квиток уже позначено як скасований.";
            return RedirectToAction(nameof(Index));
        }

        booking.Status = "Cancelled";
        dbContext.BookedSeats.RemoveRange(booking.BookedSeats);
        dbContext.NotificationLogs.Add(new Models.NotificationLog
        {
            BookingId = booking.Id,
            Email = booking.CustomerEmail,
            CreatedDate = DateTime.UtcNow,
            Status = "Emulated",
            Message = $"Запит на повернення для замовлення №{booking.Id} успішно зареєстровано. Місця знову доступні для бронювання."
        });

        await dbContext.SaveChangesAsync();
        TempData["ProfileStatusMessage"] = "Квиток успішно скасовано. Місця знову доступні для бронювання.";
        return RedirectToAction(nameof(Index));
    }

    private int? GetCurrentUserId()
    {
        return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
            ? userId
            : null;
    }
}
