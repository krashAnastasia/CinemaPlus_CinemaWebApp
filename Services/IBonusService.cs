namespace CinemaPlus.CinemaWebApp.Services;

public interface IBonusService
{
    Task<decimal> CalculateBonusAsync(int userId, CancellationToken cancellationToken = default);

    string FormatBonusLabel(decimal bonusAmount);
}
