using System.ComponentModel.DataAnnotations;

namespace CinemaPlus.CinemaWebApp.ViewModels;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Вкажіть електронну пошту.")]
    [EmailAddress(ErrorMessage = "Вкажіть коректну електронну пошту.")]
    public string Email { get; set; } = string.Empty;

    public string? StatusMessage { get; set; }

    public string? EmulatedEmailRecipient { get; set; }

    public string? EmulatedEmailSubject { get; set; }

    public string? EmulatedEmailBody { get; set; }

    public string? ResetLink { get; set; }

    public string? ExpiresAtText { get; set; }

    public bool HasEmulatedEmail => !string.IsNullOrWhiteSpace(ResetLink);
}
