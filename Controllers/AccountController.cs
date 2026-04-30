using System.Security.Claims;
using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.Models;
using CinemaPlus.CinemaWebApp.Services;
using CinemaPlus.CinemaWebApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace CinemaPlus.CinemaWebApp.Controllers;

public class AccountController(
    ApplicationDbContext dbContext,
    IPasswordService passwordService,
    IPasswordResetService passwordResetService) : Controller
{
    [HttpGet("account/login")]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectAfterLogin(User.FindFirstValue(ClaimTypes.Role));
        }

        return View(new LoginViewModel
        {
            ReturnUrl = returnUrl,
            StatusMessage = TempData["AuthStatusMessage"] as string
        });
    }

    [HttpPost("account/login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var normalizedEmail = model.Email.Trim().ToLowerInvariant();
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(account => account.Email.ToLower() == normalizedEmail);

        if (user is null || !passwordService.VerifyPassword(model.Password, user.PasswordHash))
        {
            ModelState.AddModelError(string.Empty, "Невірна електронна пошта або пароль.");
            return View(model);
        }

        await SignInAsync(user);

        if (Url.IsLocalUrl(model.ReturnUrl))
        {
            return Redirect(model.ReturnUrl);
        }

        return RedirectAfterLogin(user.Role);
    }

    [HttpGet("account/register")]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectAfterLogin(User.FindFirstValue(ClaimTypes.Role));
        }

        return View(new RegisterViewModel());
    }

    [HttpPost("account/register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var normalizedEmail = model.Email.Trim().ToLowerInvariant();
        var emailExists = await dbContext.Users
            .AnyAsync(user => user.Email.ToLower() == normalizedEmail);

        if (emailExists)
        {
            ModelState.AddModelError(nameof(model.Email), "Користувач із такою електронною поштою вже існує.");
            return View(model);
        }

        var user = new User
        {
            FullName = $"{model.FirstName.Trim()} {model.LastName.Trim()}",
            Email = normalizedEmail,
            Phone = model.Phone.Trim(),
            PasswordHash = passwordService.HashPassword(model.Password),
            Role = "Client",
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
        await SignInAsync(user);

        return RedirectToAction("Index", "Profile");
    }

    [HttpGet("account/forgot-password")]
    public IActionResult ForgotPassword()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectAfterLogin(User.FindFirstValue(ClaimTypes.Role));
        }

        return View(new ForgotPasswordViewModel());
    }

    [HttpPost("account/forgot-password")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var normalizedEmail = model.Email.Trim().ToLowerInvariant();
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(account => account.Email.ToLower() == normalizedEmail);

        var responseModel = new ForgotPasswordViewModel
        {
            Email = model.Email,
            StatusMessage = "Якщо користувач із такою електронною поштою існує, ми надіслали інструкції для відновлення пароля."
        };

        if (user is not null)
        {
            var ticket = passwordResetService.CreateTicket(user.Email, user.FullName);
            var resetLink = Url.Action(
                nameof(ResetPassword),
                "Account",
                new { email = ticket.Email, token = ticket.Token },
                Request.Scheme) ?? string.Empty;

            responseModel.EmulatedEmailRecipient = ticket.Email;
            responseModel.EmulatedEmailSubject = "Відновлення пароля CinemaPlus";
            responseModel.ExpiresAtText = ticket.ExpiresAt
                .ToLocalTime()
                .ToString("dd.MM.yyyy HH:mm", CinemaPresentationHelper.UkrainianCulture);
            responseModel.ResetLink = resetLink;
            responseModel.EmulatedEmailBody = $"Вітаємо, {ticket.FullName}! Натисніть посилання нижче, щоб встановити новий пароль для вашого акаунта CinemaPlus.";
        }

        return View(responseModel);
    }

    [HttpGet("account/reset-password")]
    public IActionResult ResetPassword(string email, string token)
    {
        var model = new ResetPasswordViewModel
        {
            Email = email,
            Token = token,
            IsTokenValid = passwordResetService.IsTokenValid(email, token)
        };

        if (!model.IsTokenValid)
        {
            model.StatusMessage = "Посилання для відновлення недійсне або вже протерміноване.";
        }

        return View(model);
    }

    [HttpPost("account/reset-password")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        model.Email = model.Email.Trim().ToLowerInvariant();
        model.IsTokenValid = passwordResetService.IsTokenValid(model.Email, model.Token);

        if (!model.IsTokenValid)
        {
            model.StatusMessage = "Посилання для відновлення недійсне або вже протерміноване.";
            return View(model);
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await dbContext.Users
            .FirstOrDefaultAsync(account => account.Email.ToLower() == model.Email);

        if (user is null)
        {
            model.IsTokenValid = false;
            model.StatusMessage = "Не вдалося знайти користувача для цього запиту на відновлення.";
            return View(model);
        }

        user.PasswordHash = passwordService.HashPassword(model.Password);
        await dbContext.SaveChangesAsync();
        passwordResetService.ConsumeToken(model.Token);

        TempData["AuthStatusMessage"] = "Пароль успішно оновлено. Тепер ви можете увійти з новим паролем.";
        return RedirectToAction(nameof(Login));
    }

    [HttpGet("account/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("account/access-denied")]
    public IActionResult AccessDenied()
    {
        Response.StatusCode = StatusCodes.Status403Forbidden;
        return View();
    }

    private async Task SignInAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            });
    }

    private IActionResult RedirectAfterLogin(string? role)
    {
        if (role == "Admin")
        {
            return RedirectToAction("Films", "Admin");
        }

        return RedirectToAction("Index", "Home");
    }
}
