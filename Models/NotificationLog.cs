namespace CinemaPlus.CinemaWebApp.Models;

public class NotificationLog
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public Booking Booking { get; set; } = null!;

    public string Email { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public string Status { get; set; } = "Emulated";
}
