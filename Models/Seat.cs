namespace CinemaPlus.CinemaWebApp.Models;

public class Seat
{
    public int Id { get; set; }

    public int HallId { get; set; }

    public Hall Hall { get; set; } = null!;

    public int RowNumber { get; set; }

    public int SeatNumber { get; set; }

    public ICollection<BookedSeat> BookedSeats { get; set; } = new List<BookedSeat>();
}
