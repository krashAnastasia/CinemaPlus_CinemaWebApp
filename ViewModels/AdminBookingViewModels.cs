namespace CinemaPlus.CinemaWebApp.ViewModels;

public class AdminBookingListViewModel
{
    public string? StatusMessage { get; set; }

    public string? ErrorMessage { get; set; }

    public string SearchQuery { get; set; } = string.Empty;

    public string SearchMode { get; set; } = "partial";

    public string StatusFilter { get; set; } = string.Empty;

    public string SortBy { get; set; } = "booking_date";

    public string SortDir { get; set; } = "desc";

    public IReadOnlyList<string> AvailableStatuses { get; set; } = [];

    public IReadOnlyList<AdminBookingListItemViewModel> Bookings { get; set; } = [];
}

public class AdminBookingListItemViewModel
{
    public int Id { get; set; }

    public int SessionId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerEmail { get; set; } = string.Empty;

    public string? CustomerPhone { get; set; }

    public string SessionSummary { get; set; } = string.Empty;

    public string SeatSummary { get; set; } = string.Empty;

    public int SeatsCount { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime BookingDate { get; set; }

    public decimal TotalPrice { get; set; }

    public bool CanCancel { get; set; }
}
