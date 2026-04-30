namespace CinemaPlus.CinemaWebApp.ViewModels;

public class MovieSessionViewModel
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public string DateText { get; set; } = string.Empty;

    public string TimeText { get; set; } = string.Empty;

    public string HallName { get; set; } = string.Empty;

    public string Technology { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
