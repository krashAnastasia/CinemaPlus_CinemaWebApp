using System.ComponentModel.DataAnnotations;

namespace CinemaPlus.CinemaWebApp.ViewModels;

public class AdminSessionListViewModel
{
    public string? StatusMessage { get; set; }

    public string? ErrorMessage { get; set; }

    public int? HallId { get; set; }

    public decimal? PriceMin { get; set; }

    public decimal? PriceMax { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string SortBy { get; set; } = "session_time";

    public string SortDir { get; set; } = "asc";

    public IReadOnlyList<AdminSessionHallOptionViewModel> Halls { get; set; } = [];

    public IReadOnlyList<AdminSessionListItemViewModel> Sessions { get; set; } = [];
}

public class AdminSessionHallOptionViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Technology { get; set; } = string.Empty;
}

public class AdminSessionListItemViewModel
{
    public int Id { get; set; }

    public string FilmTitle { get; set; } = string.Empty;

    public DateTime SessionTime { get; set; }

    public string HallName { get; set; } = string.Empty;

    public string Technology { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int BookingsCount { get; set; }

    public int SoldSeatsCount { get; set; }
}

public class AdminSessionFormViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Оберіть фільм.")]
    [Range(1, int.MaxValue, ErrorMessage = "Оберіть фільм.")]
    public int? FilmId { get; set; }

    [Required(ErrorMessage = "Оберіть зал.")]
    [Range(1, int.MaxValue, ErrorMessage = "Оберіть зал.")]
    public int? HallId { get; set; }

    [Required(ErrorMessage = "Вкажіть дату та час сеансу.")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? SessionTime { get; set; } = DateTime.Today.AddHours(18);

    [Required(ErrorMessage = "Вкажіть ціну квитка.")]
    [Range(typeof(decimal), "1", "100000", ErrorMessage = "Ціна повинна бути більшою за 0.")]
    public decimal? Price { get; set; }

    public bool HasBookings { get; set; }

    public IReadOnlyList<AdminSessionOptionViewModel> Films { get; set; } = [];

    public IReadOnlyList<AdminSessionOptionViewModel> Halls { get; set; } = [];

    public bool IsEdit => Id.HasValue;

    public string PageTitle => IsEdit ? "Редагувати сеанс" : "Додати новий сеанс";

    public string SubmitButtonText => IsEdit ? "Зберегти зміни" : "Зберегти";

    public string FormAction => IsEdit ? "EditSession" : "AddSession";
}

public class AdminSessionOptionViewModel
{
    public int Id { get; set; }

    public string Label { get; set; } = string.Empty;
}
