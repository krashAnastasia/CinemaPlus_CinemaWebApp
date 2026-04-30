using System.ComponentModel.DataAnnotations;

namespace CinemaPlus.CinemaWebApp.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Вкажіть електронну пошту.")]
    [EmailAddress(ErrorMessage = "Вкажіть коректну електронну пошту.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть пароль.")]
    public string Password { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }

    public string? StatusMessage { get; set; }
}
