namespace CinemaPlus.CinemaWebApp.ViewModels;

public class MovieDetailsViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public string Description { get; set; } = string.Empty;

    public int ReleaseYear { get; set; }

    public string AgeRestriction { get; set; } = string.Empty;

    public string PosterPath { get; set; } = string.Empty;

    public string TrailerPath { get; set; } = string.Empty;

    public bool HasTrailer { get; set; }

    public string AvailabilityText { get; set; } = string.Empty;

    public string SessionSummary { get; set; } = string.Empty;

    public bool HasAvailableSessions { get; set; }

    public int? PrimarySessionId { get; set; }

    public int? SelectedSessionId { get; set; }

    public string SelectedSessionDateLabel { get; set; } = string.Empty;

    public string SelectedSessionHallLabel { get; set; } = string.Empty;

    public string SelectedSessionPriceLabel { get; set; } = string.Empty;

    public IReadOnlyList<MovieDetailsDateChipViewModel> DateChips { get; set; } = [];

    public IReadOnlyList<MovieDetailsSessionChipViewModel> SessionChips { get; set; } = [];

    public IReadOnlyList<MovieSessionViewModel> Sessions { get; set; } = [];
}

public class MovieDetailsDateChipViewModel
{
    public int SessionId { get; set; }

    public string Label { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}

public class MovieDetailsSessionChipViewModel
{
    public int SessionId { get; set; }

    public string Label { get; set; } = string.Empty;

    public string HallLabel { get; set; } = string.Empty;

    public string PriceLabel { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
