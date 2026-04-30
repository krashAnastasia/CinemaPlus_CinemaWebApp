using CinemaPlus.CinemaWebApp.ViewModels;

namespace CinemaPlus.CinemaWebApp.Services;

public interface ICinemaTicketService
{
    string GenerateTicketCode();

    string FormatOrderNumber(int bookingId, DateTime bookingDate);

    string BuildQrPayload(TicketConfirmationViewModel ticket);

    string GenerateQrCodeDataUri(string payload);

    byte[] GenerateTicketPdf(TicketConfirmationViewModel ticket);
}
