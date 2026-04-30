namespace CinemaPlus.CinemaWebApp.ViewModels;

public class MovieCatalogueViewModel
{
    public string Title { get; set; } = string.Empty;

    public string EmptyMessage { get; set; } = string.Empty;

    public bool ShowHero { get; set; }

    public IReadOnlyList<MovieCardViewModel> Films { get; set; } = [];
}
