using System.Text.RegularExpressions;

namespace CinemaPlus.CinemaWebApp.Tests.Infrastructure;

public static partial class TestHtml
{
    public static string ExtractAntiForgeryToken(string html)
    {
        var match = AntiForgeryRegex().Match(html);
        if (!match.Success)
        {
            throw new InvalidOperationException("Не вдалося знайти antiforgery token у HTML відповіді.");
        }

        return System.Net.WebUtility.HtmlDecode(match.Groups["value"].Value);
    }

    [GeneratedRegex("<input[^>]*name=\"__RequestVerificationToken\"[^>]*value=\"(?<value>[^\"]+)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline)]
    private static partial Regex AntiForgeryRegex();
}
