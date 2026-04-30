namespace CinemaPlus.CinemaWebApp.ViewModels;

public class SeatSelectionViewModel
{
    public int SessionId { get; set; }

    public int MovieId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    public string PosterPath { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public string AgeRestriction { get; set; } = string.Empty;

    public string DateText { get; set; } = string.Empty;

    public string TimeText { get; set; } = string.Empty;

    public string HallName { get; set; } = string.Empty;

    public string Technology { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int SeatsPerRow { get; set; }

    public string SessionSummaryText { get; set; } = string.Empty;

    public IReadOnlyList<SeatSelectionDateChipViewModel> DateChips { get; set; } = [];

    public IReadOnlyList<SeatSelectionSessionChipViewModel> SessionChips { get; set; } = [];

    public IReadOnlyList<SeatSelectionRowViewModel> Rows { get; set; } = [];

    public IReadOnlyList<int> SelectedSeatIds { get; set; } = [];

    public IReadOnlyList<string> SelectedSeatLabels { get; set; } = [];

    public decimal SelectedTotalPrice { get; set; }

    public bool CanProceed => SelectedSeatIds.Count > 0;
}

public class SeatSelectionDateChipViewModel
{
    public int SessionId { get; set; }

    public string Label { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}

public class SeatSelectionSessionChipViewModel
{
    public int SessionId { get; set; }

    public string Label { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}

public class SeatSelectionRowViewModel
{
    public string Label { get; set; } = string.Empty;

    public IReadOnlyList<SeatSelectionSeatViewModel> Seats { get; set; } = [];
}

public class SeatSelectionSeatViewModel
{
    public int Id { get; set; }

    public string RowLabel { get; set; } = string.Empty;

    public int SeatNumber { get; set; }

    public bool IsBooked { get; set; }

    public bool IsSelected { get; set; }

    public string AriaLabel { get; set; } = string.Empty;

    public string CssClass { get; set; } = string.Empty;
}

public class SeatSelectionPostViewModel
{
    public List<int> SelectedSeatIds { get; set; } = [];
}
