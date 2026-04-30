using System.Globalization;
using CinemaPlus.CinemaWebApp.Models;

namespace CinemaPlus.CinemaWebApp;

public static class CinemaPresentationHelper
{
    public static readonly DateTime NowShowingCutoff = new(2026, 8, 31, 23, 59, 59);
    public static readonly DateOnly ComingSoonCutoff = new(2026, 8, 31);
    public static readonly CultureInfo UkrainianCulture = CultureInfo.GetCultureInfo("uk-UA");

    private static readonly string[] RowLabels =
    [
        "А", "Б", "В", "Г", "Д", "Е", "Є", "Ж", "З", "И",
        "І", "Ї", "Й", "К", "Л", "М", "Н", "О", "П", "Р",
        "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ь",
        "Ю", "Я"
    ];

    public static string FormatMovieTitle(Film film)
    {
        return film.Title.ToUpper(UkrainianCulture);
    }

    public static string FormatSessionSummary(IEnumerable<Session> sessions, string separator = ", ")
    {
        var sessionTimes = sessions
            .OrderBy(session => session.SessionTime)
            .Select(session => session.SessionTime.ToString("H:mm", UkrainianCulture))
            .Distinct()
            .ToList();

        return sessionTimes.Count > 0
            ? string.Join(separator, sessionTimes)
            : "Сеансів поки немає";
    }

    public static string ResolvePosterPath(Film film, IWebHostEnvironment environment)
    {
        if (StaticFileExists(film.PosterPath, environment))
        {
            return film.PosterPath;
        }

        return film.Id switch
        {
            1 => "/source/avatar-poster.webp",
            2 => "/source/images.jpeg",
            3 => "/source/it-poster.jpg",
            4 => "/source/matrix-poster.webp",
            5 => "/source/str-things-poster.jpg",
            6 => "/source/avengers-poster.jpg",
            _ => "/source/hero-img.jpg"
        };
    }

    public static string GetRowLabel(int rowNumber)
    {
        if (rowNumber >= 1 && rowNumber <= RowLabels.Length)
        {
            return RowLabels[rowNumber - 1];
        }

        return rowNumber.ToString(UkrainianCulture);
    }

    public static string FormatSeatLabel(int rowNumber, int seatNumber)
    {
        return $"Ряд {GetRowLabel(rowNumber)}, місце {seatNumber}";
    }

    private static bool StaticFileExists(string path, IWebHostEnvironment environment)
    {
        if (string.IsNullOrWhiteSpace(path) || environment.WebRootPath is null)
        {
            return false;
        }

        var relativePath = path.TrimStart('/', '\\').Replace('/', Path.DirectorySeparatorChar);
        return File.Exists(Path.Combine(environment.WebRootPath, relativePath));
    }
}
