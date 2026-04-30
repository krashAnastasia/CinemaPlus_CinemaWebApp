namespace CinemaPlus.CinemaWebApp.Models;

public class Booking
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public User? User { get; set; }

    public int SessionId { get; set; }

    public Session Session { get; set; } = null!;

    public DateTime BookingDate { get; set; }

    public string Status { get; set; } = "Paid";

    public decimal TotalPrice { get; set; }

    public string TicketCode { get; set; } = string.Empty;

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public string? CustomerPhone { get; set; }

    public ICollection<BookedSeat> BookedSeats { get; set; } = new List<BookedSeat>();

    public ICollection<NotificationLog> NotificationLogs { get; set; } = new List<NotificationLog>();
}
