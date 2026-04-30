using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;

namespace CinemaPlus.CinemaWebApp.Services;

public class MemoryCachePasswordResetService(IMemoryCache cache) : IPasswordResetService
{
    private const int ResetTokenLifetimeMinutes = 30;

    public PasswordResetTicket CreateTicket(string email, string fullName)
    {
        var normalizedEmail = NormalizeEmail(email);
        var token = WebEncoders.Base64UrlEncode(RandomNumberGenerator.GetBytes(32));
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(ResetTokenLifetimeMinutes);

        cache.Set(
            BuildCacheKey(token),
            new PasswordResetCacheEntry(normalizedEmail, fullName, expiresAt),
            expiresAt);

        return new PasswordResetTicket(normalizedEmail, fullName, token, expiresAt);
    }

    public bool IsTokenValid(string email, string token)
    {
        if (!cache.TryGetValue(BuildCacheKey(token), out PasswordResetCacheEntry? entry) || entry is null)
        {
            return false;
        }

        return entry.ExpiresAt > DateTimeOffset.UtcNow
            && string.Equals(entry.Email, NormalizeEmail(email), StringComparison.OrdinalIgnoreCase);
    }

    public void ConsumeToken(string token)
    {
        cache.Remove(BuildCacheKey(token));
    }

    private static string BuildCacheKey(string token) => $"password-reset:{token}";

    private static string NormalizeEmail(string email) => email.Trim().ToLowerInvariant();

    private sealed record PasswordResetCacheEntry(string Email, string FullName, DateTimeOffset ExpiresAt);
}
