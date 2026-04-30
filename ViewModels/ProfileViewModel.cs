namespace CinemaPlus.CinemaWebApp.ViewModels;

public class ProfileViewModel
{
    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? ProfilePhotoPath { get; set; }

    public bool UseDefaultProfilePhoto { get; set; } = true;

    public string BonusLabel { get; set; } = "0 БОНУСІВ";

    public string? StatusMessage { get; set; }

    public IReadOnlyList<ProfileTicketCardViewModel> Tickets { get; set; } = [];
}

public class ProfileTicketCardViewModel
{
    public int BookingId { get; set; }

    public string PosterPath { get; set; } = string.Empty;

    public string MovieTitle { get; set; } = string.Empty;

    public string DateText { get; set; } = string.Empty;

    public string TimeText { get; set; } = string.Empty;

    public string StatusText { get; set; } = string.Empty;

    public string SeatSummaryText { get; set; } = string.Empty;

    public bool IsCancelled { get; set; }

    public bool CanCancel { get; set; }
}
