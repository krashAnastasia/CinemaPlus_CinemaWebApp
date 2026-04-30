using System.ComponentModel.DataAnnotations;

namespace CinemaPlus.CinemaWebApp.ViewModels;

public class ResetPasswordViewModel
{
    [Required(ErrorMessage = "Вкажіть електронну пошту.")]
    [EmailAddress(ErrorMessage = "Вкажіть коректну електронну пошту.")]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть новий пароль.")]
    [MinLength(6, ErrorMessage = "Пароль має містити щонайменше 6 символів.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Підтвердіть новий пароль.")]
    [Compare(nameof(Password), ErrorMessage = "Паролі не збігаються.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public bool IsTokenValid { get; set; } = true;

    public string? StatusMessage { get; set; }
}
