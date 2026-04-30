namespace CinemaPlus.CinemaWebApp.ViewModels;

public class AdminPopularFilmsStatsViewModel
{
    public IReadOnlyList<AdminPopularFilmsStatsItemViewModel> Items { get; set; } = [];
}

public class AdminPopularFilmsStatsItemViewModel
{
    public string FilmTitle { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int SessionsCount { get; set; }

    public int BookingsCount { get; set; }

    public int SoldSeatsCount { get; set; }
}

public class AdminRevenueViewModel
{
    public string? ErrorMessage { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal TotalRevenue { get; set; }

    public int TotalSoldSeats { get; set; }

    public int TotalPaidBookings { get; set; }

    public IReadOnlyList<AdminRevenueSessionItemViewModel> Sessions { get; set; } = [];
}

public class AdminRevenueSessionItemViewModel
{
    public int SessionId { get; set; }

    public DateTime SessionTime { get; set; }

    public string FilmTitle { get; set; } = string.Empty;

    public string HallName { get; set; } = string.Empty;

    public string Technology { get; set; } = string.Empty;

    public decimal TicketPrice { get; set; }

    public int SoldSeatsCount { get; set; }

    public decimal Revenue { get; set; }
}

public class AdminReportsViewModel
{
    public string? StatusMessage { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int TotalSessions { get; set; }

    public int TotalPaidBookings { get; set; }

    public int TotalSoldSeats { get; set; }

    public decimal TotalRevenue { get; set; }
}
