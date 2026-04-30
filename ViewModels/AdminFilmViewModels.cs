using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CinemaPlus.CinemaWebApp.ViewModels;

public class AdminFilmListViewModel
{
    public string SearchQuery { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int? YearMin { get; set; }

    public int? YearMax { get; set; }

    public string SortBy { get; set; } = "film_title";

    public string SortDir { get; set; } = "asc";

    public string? StatusMessage { get; set; }

    public string? ErrorMessage { get; set; }

    public IReadOnlyList<string> Genres { get; set; } = [];

    public IReadOnlyList<AdminFilmListItemViewModel> Films { get; set; } = [];
}

public class AdminFilmListItemViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public string Description { get; set; } = string.Empty;

    public int ReleaseYear { get; set; }

    public string AgeRestriction { get; set; } = string.Empty;

    public string PosterPath { get; set; } = string.Empty;
}

public class AdminFilmFormViewModel : IValidatableObject
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Вкажіть назву фільму.")]
    [StringLength(200, ErrorMessage = "Назва не може містити більше 200 символів.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть жанр.")]
    [StringLength(100, ErrorMessage = "Жанр не може містити більше 100 символів.")]
    public string Genre { get; set; } = string.Empty;

    [Range(1, 500, ErrorMessage = "Тривалість повинна бути в межах від 1 до 500 хвилин.")]
    public int DurationMinutes { get; set; }

    [Required(ErrorMessage = "Вкажіть опис фільму.")]
    public string Description { get; set; } = string.Empty;

    [Range(1900, 2100, ErrorMessage = "Вкажіть коректний рік випуску.")]
    public int ReleaseYear { get; set; }

    [Required(ErrorMessage = "Вкажіть вікове обмеження.")]
    [StringLength(20, ErrorMessage = "Вікове обмеження не може містити більше 20 символів.")]
    public string AgeRestriction { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть дату доступності.")]
    [DataType(DataType.Date)]
    public DateOnly AvailabilityDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required(ErrorMessage = "Оберіть статус показу.")]
    [RegularExpression("NowShowing|ComingSoon", ErrorMessage = "Оберіть коректний статус показу.")]
    public string AvailabilityStatus { get; set; } = "NowShowing";

    public string CurrentPosterPath { get; set; } = string.Empty;

    public string CurrentTrailerPath { get; set; } = string.Empty;

    public IFormFile? PosterFile { get; set; }

    public IFormFile? TrailerFile { get; set; }

    public bool HasCurrentTrailer => !string.IsNullOrWhiteSpace(CurrentTrailerPath);

    public bool IsEdit => Id.HasValue;

    public string PageTitle => IsEdit ? "Редагувати фільм" : "Додати новий фільм";

    public string SubmitButtonText => IsEdit ? "Зберегти зміни" : "Зберегти фільм";

    public string FormAction => IsEdit ? "EditFilm" : "AddFilm";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AvailabilityStatus == "ComingSoon" && AvailabilityDate <= CinemaPresentationHelper.ComingSoonCutoff)
        {
            yield return new ValidationResult(
                "Для статусу «СКОРО» дата доступності повинна бути пізнішою за 31.08.2026.",
                [nameof(AvailabilityDate)]);
        }

        if (AvailabilityStatus == "NowShowing" && AvailabilityDate > CinemaPresentationHelper.ComingSoonCutoff)
        {
            yield return new ValidationResult(
                "Для статусу «У КІНО» дата доступності повинна бути не пізнішою за 31.08.2026.",
                [nameof(AvailabilityDate)]);
        }
    }
}
