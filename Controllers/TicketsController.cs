using System.Security.Claims;
using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.Models;
using CinemaPlus.CinemaWebApp.Services;
using CinemaPlus.CinemaWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Controllers;

public class TicketsController(
    ApplicationDbContext dbContext,
    IWebHostEnvironment environment,
    ICinemaTicketService ticketService) : Controller
{
    [HttpGet("tickets/{id:int}")]
    public async Task<IActionResult> Details(int id, string? ticketCode)
    {
        var booking = await LoadBookingAsync(id);
        if (booking is null)
        {
            return NotFound();
        }

        var accessResult = ValidateAccess(booking, ticketCode);
        if (accessResult is not null)
        {
            return accessResult;
        }

        var viewModel = BuildTicketViewModel(booking, ticketCode);
        return View(viewModel);
    }

    [HttpGet("tickets/{id:int}/pdf")]
    public async Task<IActionResult> Pdf(int id, string? ticketCode)
    {
        var booking = await LoadBookingAsync(id);
        if (booking is null)
        {
            return NotFound();
        }

        var accessResult = ValidateAccess(booking, ticketCode);
        if (accessResult is not null)
        {
            return accessResult;
        }

        var viewModel = BuildTicketViewModel(booking, ticketCode);
        var pdfBytes = ticketService.GenerateTicketPdf(viewModel);
        var fileName = $"cinemaplus-ticket-{viewModel.OrderNumber}.pdf";

        return File(pdfBytes, "application/pdf", fileName);
    }

    private async Task<Booking?> LoadBookingAsync(int bookingId)
    {
        return await dbContext.Bookings
            .AsNoTracking()
            .AsSplitQuery()
            .Include(booking => booking.Session)
                .ThenInclude(session => session.Film)
            .Include(booking => booking.Session)
                .ThenInclude(session => session.Hall)
            .Include(booking => booking.BookedSeats)
                .ThenInclude(bookedSeat => bookedSeat.Seat)
            .FirstOrDefaultAsync(booking => booking.Id == bookingId);
    }

    private IActionResult? ValidateAccess(Booking booking, string? ticketCode)
    {
        if (User.IsInRole("Admin"))
        {
            return null;
        }

        var currentUserId = GetCurrentUserId();
        if (currentUserId.HasValue)
        {
            return booking.UserId == currentUserId.Value ? null : Forbid();
        }

        if (booking.UserId.HasValue)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = $"{Request.Path}{Request.QueryString}" });
        }

        return string.Equals(ticketCode, booking.TicketCode, StringComparison.Ordinal)
            ? null
            : NotFound();
    }

    private TicketConfirmationViewModel BuildTicketViewModel(Booking booking, string? ticketCode)
    {
        var orderedSeats = booking.BookedSeats
            .Select(item => item.Seat)
            .OrderBy(seat => seat.RowNumber)
            .ThenBy(seat => seat.SeatNumber)
            .ToList();

        var seatLabels = orderedSeats
            .Select(seat => CinemaPresentationHelper.FormatSeatLabel(seat.RowNumber, seat.SeatNumber))
            .ToList();

        var seatPrimaryLine = orderedSeats.Count == 1
            ? $"РЯД {CinemaPresentationHelper.GetRowLabel(orderedSeats[0].RowNumber)}"
            : "МІСЦЯ";

        var seatSecondaryLine = orderedSeats.Count == 1
            ? $"МІСЦЕ {orderedSeats[0].SeatNumber}"
            : string.Join(" • ", seatLabels);

        var viewModel = new TicketConfirmationViewModel
        {
            BookingId = booking.Id,
            OrderNumber = ticketService.FormatOrderNumber(booking.Id, booking.BookingDate),
            TicketCode = booking.TicketCode,
            AccessTicketCode = booking.UserId.HasValue ? string.Empty : ticketCode ?? booking.TicketCode,
            PosterPath = CinemaPresentationHelper.ResolvePosterPath(booking.Session.Film, environment),
            MovieTitle = CinemaPresentationHelper.FormatMovieTitle(booking.Session.Film),
            Genre = booking.Session.Film.Genre,
            DurationMinutes = booking.Session.Film.DurationMinutes,
            DateText = booking.Session.SessionTime.ToString("dd.MM.yyyy", CinemaPresentationHelper.UkrainianCulture),
            TimeText = booking.Session.SessionTime.ToString("H:mm", CinemaPresentationHelper.UkrainianCulture),
            HallName = booking.Session.Hall.Name,
            Technology = booking.Session.Hall.Technology,
            Quantity = orderedSeats.Count,
            SeatSummaryText = string.Join(" • ", seatLabels),
            TicketSeatLinePrimary = seatPrimaryLine,
            TicketSeatLineSecondary = seatSecondaryLine,
            TotalPrice = booking.TotalPrice,
            CustomerEmail = booking.CustomerEmail
        };

        viewModel.QrPayload = ticketService.BuildQrPayload(viewModel);
        viewModel.QrCodeDataUri = ticketService.GenerateQrCodeDataUri(viewModel.QrPayload);

        return viewModel;
    }

    private int? GetCurrentUserId()
    {
        return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
            ? userId
            : null;
    }
}
