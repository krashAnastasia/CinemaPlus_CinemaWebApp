using System.Security.Claims;
using CinemaPlus.CinemaWebApp.Data;
using CinemaPlus.CinemaWebApp.Models;
using CinemaPlus.CinemaWebApp.Services;
using CinemaPlus.CinemaWebApp.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaPlus.CinemaWebApp.Controllers;

[Authorize]
public class ProfileController(
    ApplicationDbContext dbContext,
    IConfiguration configuration,
    IWebHostEnvironment environment,
    IBonusService bonusService) : Controller
{
    private static readonly HashSet<string> AllowedProfilePhotoExtensions =
    [
        ".jpg",
        ".jpeg",
        ".png",
        ".webp"
    ];

    private const long MaxProfilePhotoBytes = 5L * 1024L * 1024L;

    [Route("profile")]
    public async Task<IActionResult> Index()
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = "/profile" });
        }

        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == currentUserId.Value);

        if (user is null)
        {
            return NotFound();
        }

        return View(await BuildProfileViewModelAsync(user, currentUserId.Value));
    }

    [HttpPost("profile/edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = "/profile" });
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(item => item.Id == currentUserId.Value);
        if (user is null)
        {
            return NotFound();
        }

        model.FullName = model.FullName.Trim();
        model.Email = model.Email.Trim().ToLowerInvariant();
        model.Phone = model.Phone.Trim();
        model.IsEditingProfile = true;

        if (!string.IsNullOrWhiteSpace(model.Email))
        {
            var emailExists = await dbContext.Users.AnyAsync(item =>
                item.Id != currentUserId.Value
                && item.Email.ToLower() == model.Email);

            if (emailExists)
            {
                ModelState.AddModelError(nameof(model.Email), "Користувач із такою електронною поштою вже існує.");
            }
        }

        if (!ModelState.IsValid)
        {
            var invalidViewModel = await BuildProfileViewModelAsync(user, currentUserId.Value, model);
            invalidViewModel.IsEditingProfile = true;
            return View("Index", invalidViewModel);
        }

        user.FullName = model.FullName;
        user.Email = model.Email;
        user.Phone = string.IsNullOrWhiteSpace(model.Phone) ? null : model.Phone;

        await dbContext.SaveChangesAsync();
        await RefreshAuthenticationAsync(user);

        TempData["ProfileStatusMessage"] = "Дані профілю успішно оновлено.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("profile/photo")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfilePhoto(IFormFile? profilePhotoFile)
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = "/profile" });
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(item => item.Id == currentUserId.Value);
        if (user is null)
        {
            return NotFound();
        }

        if (profilePhotoFile is null)
        {
            TempData["ProfileErrorMessage"] = "Оберіть фото профілю перед збереженням.";
            return RedirectToAction(nameof(Index));
        }

        var extension = Path.GetExtension(profilePhotoFile.FileName).ToLowerInvariant();
        if (!AllowedProfilePhotoExtensions.Contains(extension))
        {
            TempData["ProfileErrorMessage"] = "Дозволено лише зображення JPG, PNG або WebP.";
            return RedirectToAction(nameof(Index));
        }

        if (profilePhotoFile.Length == 0)
        {
            TempData["ProfileErrorMessage"] = "Файл фото порожній. Оберіть інший файл.";
            return RedirectToAction(nameof(Index));
        }

        if (profilePhotoFile.Length > MaxProfilePhotoBytes)
        {
            TempData["ProfileErrorMessage"] = "Фото профілю перевищує ліміт у 5 МБ.";
            return RedirectToAction(nameof(Index));
        }

        var previousPhotoPath = user.ProfilePhotoPath;
        user.ProfilePhotoPath = await SaveProfilePhotoAsync(profilePhotoFile, user.FullName);
        await dbContext.SaveChangesAsync();

        if (!string.Equals(previousPhotoPath, user.ProfilePhotoPath, StringComparison.Ordinal))
        {
            DeleteManagedProfilePhoto(previousPhotoPath);
        }

        TempData["ProfileStatusMessage"] = "Фото профілю успішно оновлено.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("profile/bookings/{bookingId:int}/cancel")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelBooking(int bookingId)
    {
        var currentUserId = GetCurrentUserId();
        if (!currentUserId.HasValue)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = "/profile" });
        }

        var booking = await dbContext.Bookings
            .Include(item => item.BookedSeats)
            .FirstOrDefaultAsync(item => item.Id == bookingId && item.UserId == currentUserId.Value);

        if (booking is null)
        {
            return NotFound();
        }

        if (booking.Status == "Cancelled")
        {
            TempData["ProfileStatusMessage"] = "Цей квиток уже позначено як скасований.";
            return RedirectToAction(nameof(Index));
        }

        booking.Status = "Cancelled";
        dbContext.BookedSeats.RemoveRange(booking.BookedSeats);
        dbContext.NotificationLogs.Add(new Models.NotificationLog
        {
            BookingId = booking.Id,
            Email = booking.CustomerEmail,
            CreatedDate = DateTime.UtcNow,
            Status = "Emulated",
            Message = $"Запит на повернення для замовлення №{booking.Id} успішно зареєстровано. Місця знову доступні для бронювання."
        });

        await dbContext.SaveChangesAsync();
        TempData["ProfileStatusMessage"] = "Квиток успішно скасовано. Місця знову доступні для бронювання.";
        return RedirectToAction(nameof(Index));
    }

    private int? GetCurrentUserId()
    {
        return int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
            ? userId
            : null;
    }

    private async Task<ProfileViewModel> BuildProfileViewModelAsync(User user, int userId, ProfileViewModel? sourceModel = null)
    {
        var bookings = await dbContext.Bookings
            .AsNoTracking()
            .AsSplitQuery()
            .Where(item => item.UserId == userId)
            .Include(item => item.Session)
                .ThenInclude(session => session.Film)
            .Include(item => item.Session)
                .ThenInclude(session => session.Hall)
            .Include(item => item.BookedSeats)
                .ThenInclude(bookedSeat => bookedSeat.Seat)
            .OrderBy(item => item.Status == "Cancelled")
            .ThenByDescending(item => item.Session.SessionTime)
            .ThenByDescending(item => item.BookingDate)
            .ToListAsync();

        return new ProfileViewModel
        {
            FullName = sourceModel?.FullName ?? user.FullName,
            Email = sourceModel?.Email ?? user.Email,
            Phone = sourceModel?.Phone ?? user.Phone ?? string.Empty,
            ProfilePhotoPath = user.ProfilePhotoPath,
            UseDefaultProfilePhoto = string.IsNullOrWhiteSpace(user.ProfilePhotoPath),
            BonusLabel = bonusService.FormatBonusLabel(await bonusService.CalculateBonusAsync(userId)),
            StatusMessage = TempData["ProfileStatusMessage"] as string,
            ErrorMessage = TempData["ProfileErrorMessage"] as string,
            IsEditingProfile = sourceModel?.IsEditingProfile ?? false,
            Tickets = bookings.Select(booking =>
            {
                var orderedSeats = booking.BookedSeats
                    .Select(item => item.Seat)
                    .OrderBy(seat => seat.RowNumber)
                    .ThenBy(seat => seat.SeatNumber)
                    .ToList();

                return new ProfileTicketCardViewModel
                {
                    BookingId = booking.Id,
                    PosterPath = CinemaPresentationHelper.ResolvePosterPath(booking.Session.Film, environment),
                    MovieTitle = CinemaPresentationHelper.FormatMovieTitle(booking.Session.Film),
                    DateText = booking.Session.SessionTime.ToString("dd.MM.yyyy", CinemaPresentationHelper.UkrainianCulture),
                    TimeText = booking.Session.SessionTime.ToString("H:mm", CinemaPresentationHelper.UkrainianCulture),
                    StatusText = booking.Status == "Cancelled" ? "СКАСОВАНО" : "АКТИВНИЙ",
                    SeatSummaryText = orderedSeats.Count > 0
                        ? string.Join(" • ", orderedSeats.Select(seat => CinemaPresentationHelper.FormatSeatLabel(seat.RowNumber, seat.SeatNumber)))
                        : "Місця звільнено",
                    IsCancelled = booking.Status == "Cancelled",
                    CanCancel = booking.Status != "Cancelled"
                };
            }).ToList()
        };
    }

    private async Task RefreshAuthenticationAsync(User user)
    {
        var authenticationService = HttpContext.RequestServices.GetService<IAuthenticationService>();
        if (authenticationService is null)
        {
            return;
        }

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

    private async Task<string> SaveProfilePhotoAsync(IFormFile photoFile, string fullName)
    {
        var uploadsRelativePath = configuration["Storage:UploadsPath"] ?? "wwwroot/uploads";
        var uploadsDirectory = Path.IsPathRooted(uploadsRelativePath)
            ? uploadsRelativePath
            : Path.Combine(environment.ContentRootPath, uploadsRelativePath);

        Directory.CreateDirectory(uploadsDirectory);

        var extension = Path.GetExtension(photoFile.FileName).ToLowerInvariant();
        var safeBaseName = SanitizeFileName(fullName);
        var fileName = $"profile-{safeBaseName}-{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsDirectory, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await photoFile.CopyToAsync(stream);

        return $"/uploads/{fileName}";
    }

    private void DeleteManagedProfilePhoto(string? profilePhotoPath)
    {
        if (!IsManagedProfilePhoto(profilePhotoPath) || environment.WebRootPath is null)
        {
            return;
        }

        var relativePath = profilePhotoPath!.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(environment.WebRootPath, relativePath);
        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }

    private static bool IsManagedProfilePhoto(string? profilePhotoPath)
    {
        return !string.IsNullOrWhiteSpace(profilePhotoPath)
            && profilePhotoPath.StartsWith("/uploads/profile-", StringComparison.OrdinalIgnoreCase);
    }

    private static string SanitizeFileName(string value)
    {
        var sanitized = new string(value
            .ToLowerInvariant()
            .Select(ch => char.IsLetterOrDigit(ch) ? ch : '-')
            .ToArray())
            .Trim('-');

        return string.IsNullOrWhiteSpace(sanitized) ? "user" : sanitized;
    }
}
