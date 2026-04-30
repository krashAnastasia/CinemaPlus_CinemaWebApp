using PdfSharpCore.Fonts;

namespace CinemaPlus.CinemaWebApp.Services;

internal sealed class CinemaPlusFontResolver : IFontResolver
{
    private const string FontFamilyName = "CinemaPlusSans";
    private const string RegularFaceName = "CinemaPlusSans-Regular";
    private const string BoldFaceName = "CinemaPlusSans-Bold";

    private static readonly Lazy<byte[]> RegularFontData = new(() =>
        File.ReadAllBytes("/System/Library/Fonts/Supplemental/Arial.ttf"));

    private static readonly Lazy<byte[]> BoldFontData = new(() =>
        File.ReadAllBytes("/System/Library/Fonts/Supplemental/Arial Bold.ttf"));

    public string DefaultFontName => FontFamilyName;

    public byte[]? GetFont(string faceName)
    {
        return faceName switch
        {
            RegularFaceName => RegularFontData.Value,
            BoldFaceName => BoldFontData.Value,
            _ => null
        };
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        var normalizedFamily = familyName?.Trim() ?? string.Empty;

        if (!string.Equals(normalizedFamily, FontFamilyName, StringComparison.OrdinalIgnoreCase))
        {
            normalizedFamily = FontFamilyName;
        }

        return new FontResolverInfo(isBold ? BoldFaceName : RegularFaceName);
    }
}
