using System.Text.RegularExpressions;

namespace LuxeVoyage.Mvc.Helpers;

public static class CatalogTextHelper
{
    /// <summary>Splits comma-, newline-, or semicolon-separated entries used for gallery URLs and highlights.</summary>
    public static IReadOnlyList<string> SplitList(string? csv)
    {
        if (string.IsNullOrWhiteSpace(csv))
            return Array.Empty<string>();
        return csv
            .Split(new[] { ',', ';', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .ToList();
    }

    /// <summary>Lowercase slug with allowed URL-safe characters only.</summary>
    public static string NormalizeSlug(string? slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return "";
        var s = slug.Trim().ToLowerInvariant();
        s = Regex.Replace(s, @"[^a-z0-9\-]", "-", RegexOptions.CultureInvariant);
        s = Regex.Replace(s, "-{2,}", "-", RegexOptions.CultureInvariant);
        return s.Trim('-');
    }
}
