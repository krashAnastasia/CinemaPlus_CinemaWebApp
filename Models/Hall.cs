namespace CinemaPlus.CinemaWebApp.Models;

public class Hall
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Technology { get; set; } = string.Empty;

    public int RowsCount { get; set; }

    public int SeatsPerRow { get; set; }

    public ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}
