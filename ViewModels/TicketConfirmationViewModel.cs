namespace CinemaPlus.CinemaWebApp.ViewModels;

public class TicketConfirmationViewModel
{
    public int BookingId { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public string TicketCode { get; set; } = string.Empty;

    public string AccessTicketCode { get; set; } = string.Empty;

    public string PosterPath { get; set; } = string.Empty;

    public string MovieTitle { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public string DateText { get; set; } = string.Empty;

    public string TimeText { get; set; } = string.Empty;

    public string HallName { get; set; } = string.Empty;

    public string Technology { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public string SeatSummaryText { get; set; } = string.Empty;

    public string TicketSeatLinePrimary { get; set; } = string.Empty;

    public string TicketSeatLineSecondary { get; set; } = string.Empty;

    public decimal TotalPrice { get; set; }

    public string CustomerEmail { get; set; } = string.Empty;

    public string QrCodeDataUri { get; set; } = string.Empty;

    public string QrPayload { get; set; } = string.Empty;

    public bool RequiresGuestTicketCode => !string.IsNullOrWhiteSpace(AccessTicketCode);
}
