namespace CinemaPlus.CinemaWebApp.Models;

public class Session
{
    public int Id { get; set; }

    public int FilmId { get; set; }

    public Film Film { get; set; } = null!;

    public int HallId { get; set; }

    public Hall Hall { get; set; } = null!;

    public DateTime SessionTime { get; set; }

    public decimal Price { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public ICollection<BookedSeat> BookedSeats { get; set; } = new List<BookedSeat>();
}
