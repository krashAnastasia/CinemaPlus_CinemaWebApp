using CinemaPlus.CinemaWebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace CinemaPlus.CinemaWebApp.Services;

public class BonusService(ApplicationDbContext dbContext) : IBonusService
{
    public async Task<decimal> CalculateBonusAsync(int userId, CancellationToken cancellationToken = default)
    {
        var totalSpent = await dbContext.Bookings
            .AsNoTracking()
            .Where(booking => booking.UserId == userId && booking.Status != "Cancelled")
            .SumAsync(booking => (decimal?)booking.TotalPrice, cancellationToken) ?? 0m;

        return totalSpent / 2m;
    }

    public string FormatBonusLabel(decimal bonusAmount)
    {
        var format = decimal.Truncate(bonusAmount) == bonusAmount ? "0" : "0.##";
        return $"{bonusAmount.ToString(format, CinemaPresentationHelper.UkrainianCulture)} БОНУСІВ";
    }
}
