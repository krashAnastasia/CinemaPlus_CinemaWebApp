using System.ComponentModel.DataAnnotations;

namespace CinemaPlus.CinemaWebApp.ViewModels;

public class CheckoutSelectionViewModel
{
    public int SessionId { get; set; }

    public int MovieId { get; set; }

    public string PosterPath { get; set; } = string.Empty;

    public string MovieTitle { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public int DurationMinutes { get; set; }

    public string AgeRestriction { get; set; } = string.Empty;

    public string DateText { get; set; } = string.Empty;

    public string TimeText { get; set; } = string.Empty;

    public string HallName { get; set; } = string.Empty;

    public decimal PricePerSeat { get; set; }

    public decimal TotalPrice { get; set; }

    public List<int> SelectedSeatIds { get; set; } = [];

    public List<string> SeatLabels { get; set; } = [];

    [Required(ErrorMessage = "Вкажіть ім'я та прізвище.")]
    [StringLength(150, ErrorMessage = "Ім'я не може містити більше 150 символів.")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть електронну адресу.")]
    [EmailAddress(ErrorMessage = "Вкажіть коректну електронну адресу.")]
    [StringLength(180, ErrorMessage = "Електронна адреса не може містити більше 180 символів.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть номер телефону.")]
    [StringLength(30, ErrorMessage = "Номер телефону не може містити більше 30 символів.")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть номер картки.")]
    [RegularExpression(@"(?:\d{4}\s){3}\d{4}|\d{16}", ErrorMessage = "Вкажіть коректний номер картки.")]
    public string CardNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть термін дії картки.")]
    [RegularExpression(@"(0[1-9]|1[0-2])\/\d{2}", ErrorMessage = "Вкажіть термін дії у форматі MM/YY.")]
    public string CardExpiry { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть код CVC.")]
    [RegularExpression(@"\d{3}", ErrorMessage = "Вкажіть коректний код CVC.")]
    public string CardCvc { get; set; } = string.Empty;

    public bool IsAuthenticatedUser { get; set; }

    public bool ProfileEditorOpen { get; set; }

    public string PaymentMethod { get; set; } = "Card";

    public int Quantity => SeatLabels.Count;

    public bool CanSubmit => SelectedSeatIds.Count > 0;

    public string SeatSummaryText => SeatLabels.Count > 0
        ? string.Join(" • ", SeatLabels)
        : "Місця не обрано";
}
