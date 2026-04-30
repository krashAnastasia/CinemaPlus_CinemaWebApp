namespace CinemaPlus.CinemaWebApp.Models;

public class Film
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public string Description { get; set; } = string.Empty;

    public int ReleaseYear { get; set; }

    public string AgeRestriction { get; set; } = string.Empty;

    public string PosterPath { get; set; } = string.Empty;

    public string? TrailerPath { get; set; }

    public DateOnly AvailabilityDate { get; set; }

    public string AvailabilityStatus { get; set; } = "NowShowing";

    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}
