using System.ComponentModel.DataAnnotations;

namespace CinemaPlus.CinemaWebApp.ViewModels;

public class ProfileViewModel
{
    [Required(ErrorMessage = "Вкажіть ваше ім'я та прізвище.")]
    [StringLength(120, ErrorMessage = "Ім'я не може містити більше 120 символів.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть електронну пошту.")]
    [EmailAddress(ErrorMessage = "Вкажіть коректну електронну пошту.")]
    [StringLength(150, ErrorMessage = "Електронна пошта не може містити більше 150 символів.")]
    public string Email { get; set; } = string.Empty;

    [RegularExpression(@"^[0-9+()\-\s]{7,20}$", ErrorMessage = "Вкажіть коректний номер телефону.")]
    public string Phone { get; set; } = string.Empty;

    public string? ProfilePhotoPath { get; set; }

    public bool UseDefaultProfilePhoto { get; set; } = true;

    public string BonusLabel { get; set; } = "0 БОНУСІВ";

    public string? StatusMessage { get; set; }

    public string? ErrorMessage { get; set; }

    public bool IsEditingProfile { get; set; }

    public IReadOnlyList<ProfileTicketCardViewModel> Tickets { get; set; } = [];

    public string PhoneDisplay => string.IsNullOrWhiteSpace(Phone) ? "Не вказано" : Phone;
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
