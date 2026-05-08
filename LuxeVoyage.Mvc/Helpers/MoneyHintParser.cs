using System.Globalization;
using System.Text.RegularExpressions;

namespace LuxeVoyage.Mvc.Helpers;

/// <summary>Parses optional currency hints from catalog string fields (e.g. "From $450" / "$1,200").</summary>
public static class MoneyHintParser
{
    public static decimal? TryParseUsd(string? text)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            var m = Regex.Match(text, @"\$\s*([\d,]+(?:\.\d+)?)|([\d,]+(?:\.\d+)?)\s*(?:USD|usd)?");
            if (!m.Success)
                return null;

            var raw = !string.IsNullOrEmpty(m.Groups[1].Value) ? m.Groups[1].Value : m.Groups[2].Value;
            raw = raw.Replace(",", "", StringComparison.Ordinal);
            return decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out var v)
                ? v
                : null;
        }
        catch
        {
            return null;
        }
    }
}
