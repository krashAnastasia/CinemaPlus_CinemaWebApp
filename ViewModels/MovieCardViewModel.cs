namespace CinemaPlus.CinemaWebApp.ViewModels;

public class MovieCardViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string PosterPath { get; set; } = string.Empty;

    public string SessionSummary { get; set; } = string.Empty;

    public bool HasSessions { get; set; }
}
