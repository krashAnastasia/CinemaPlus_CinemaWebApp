using System.ComponentModel.DataAnnotations;

namespace CinemaPlus.CinemaWebApp.ViewModels;

public class AdminHallListViewModel
{
    public string? StatusMessage { get; set; }

    public string? ErrorMessage { get; set; }

    public IReadOnlyList<AdminHallListItemViewModel> Halls { get; set; } = [];
}

public class AdminHallListItemViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Technology { get; set; } = string.Empty;

    public int RowsCount { get; set; }

    public int SeatsPerRow { get; set; }

    public int TotalSeats { get; set; }

    public int SessionsCount { get; set; }

    public bool LayoutLocked { get; set; }
}

public class AdminHallFormViewModel
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Вкажіть назву залу.")]
    [StringLength(80, ErrorMessage = "Назва залу не може містити більше 80 символів.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть технологію залу.")]
    [StringLength(80, ErrorMessage = "Технологія не може містити більше 80 символів.")]
    public string Technology { get; set; } = string.Empty;

    [Range(1, 32, ErrorMessage = "Кількість рядів повинна бути в межах від 1 до 32.")]
    public int RowsCount { get; set; }

    [Range(1, 40, ErrorMessage = "Кількість місць у ряду повинна бути в межах від 1 до 40.")]
    public int SeatsPerRow { get; set; }

    public bool HasProtectedBookings { get; set; }

    public bool IsEdit => Id.HasValue;

    public string PageTitle => IsEdit ? "Редагувати зал" : "Додати зал";

    public string SubmitButtonText => IsEdit ? "Зберегти зміни" : "Створити зал";

    public string FormAction => IsEdit ? "EditHall" : "AddHall";

    public int TotalSeats => RowsCount * SeatsPerRow;
}
