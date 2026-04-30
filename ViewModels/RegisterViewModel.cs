using System.ComponentModel.DataAnnotations;

namespace CinemaPlus.CinemaWebApp.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Вкажіть ім'я.")]
    [StringLength(80, ErrorMessage = "Ім'я має містити не більше 80 символів.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть прізвище.")]
    [StringLength(80, ErrorMessage = "Прізвище має містити не більше 80 символів.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть електронну пошту.")]
    [EmailAddress(ErrorMessage = "Вкажіть коректну електронну пошту.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть номер телефону.")]
    [Phone(ErrorMessage = "Вкажіть коректний номер телефону.")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Вкажіть пароль.")]
    [MinLength(6, ErrorMessage = "Пароль має містити щонайменше 6 символів.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Підтвердіть пароль.")]
    [Compare(nameof(Password), ErrorMessage = "Паролі не збігаються.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
