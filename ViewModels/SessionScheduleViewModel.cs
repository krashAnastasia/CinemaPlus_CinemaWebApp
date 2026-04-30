namespace CinemaPlus.CinemaWebApp.ViewModels;

public class SessionScheduleViewModel
{
    public string SelectedDateValue { get; set; } = string.Empty;

    public string SelectedDateDisplayValue { get; set; } = string.Empty;

    public int? SelectedMovieId { get; set; }

    public int? SelectedHallId { get; set; }

    public string EmptyMessage { get; set; } = string.Empty;

    public IReadOnlyList<SessionFilterOptionViewModel> MovieOptions { get; set; } = [];

    public IReadOnlyList<SessionFilterOptionViewModel> HallOptions { get; set; } = [];

    public IReadOnlyList<SessionScheduleItemViewModel> Sessions { get; set; } = [];
}

public class SessionFilterOptionViewModel
{
    public int Id { get; set; }

    public string Label { get; set; } = string.Empty;
}

public class SessionScheduleItemViewModel
{
    public int SessionId { get; set; }

    public int MovieId { get; set; }

    public string PosterPath { get; set; } = string.Empty;

    public string MovieTitle { get; set; } = string.Empty;

    public string DateText { get; set; } = string.Empty;

    public string TimeText { get; set; } = string.Empty;

    public string HallName { get; set; } = string.Empty;

    public string Technology { get; set; } = string.Empty;

    public decimal Price { get; set; }
}
