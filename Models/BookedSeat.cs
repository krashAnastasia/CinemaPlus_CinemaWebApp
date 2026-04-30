namespace CinemaPlus.CinemaWebApp.Models;

public class BookedSeat
{
    public int Id { get; set; }

    public int BookingId { get; set; }

    public Booking Booking { get; set; } = null!;

    public int SessionId { get; set; }

    public Session Session { get; set; } = null!;

    public int SeatId { get; set; }

    public Seat Seat { get; set; } = null!;
}
