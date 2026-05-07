using System.Globalization;
using System.Text.RegularExpressions;

namespace LuxeVoyage.Mvc.Helpers;

public static class GuestCapacityParser
{
    private static readonly Regex NumberRegex = new(@"\d+", RegexOptions.Compiled);

    public static bool TryParseGuestRange(string? input, out int min, out int max)
    {
        min = 0;
        max = 0;
        if (string.IsNullOrWhiteSpace(input))
            return false;

        var normalized = input.Replace('–', '-').Replace('—', '-').Trim();
        var matches = NumberRegex.Matches(normalized);
        if (matches.Count == 0)
            return false;

        if (!int.TryParse(matches[0].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out min))
            return false;

        if (matches.Count >= 2 &&
            int.TryParse(matches[1].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedMax))
        {
            max = parsedMax;
        }
        else
        {
            max = min;
        }

        if (min <= 0 || max <= 0)
            return false;

        if (min > max)
            (min, max) = (max, min);

        return true;
    }
}
