namespace CinemaPlus.CinemaWebApp.Services;

public interface IPasswordResetService
{
    PasswordResetTicket CreateTicket(string email, string fullName);

    bool IsTokenValid(string email, string token);

    void ConsumeToken(string token);
}

public sealed record PasswordResetTicket(
    string Email,
    string FullName,
    string Token,
    DateTimeOffset ExpiresAt);
