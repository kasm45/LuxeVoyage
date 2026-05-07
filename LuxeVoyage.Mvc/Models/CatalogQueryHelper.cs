namespace LuxeVoyage.Mvc.Models;

public static class CatalogQueryHelper
{
    public static ExperienceCategoryKind? ParseCategory(string? key)
    {
        if (string.IsNullOrWhiteSpace(key)) return null;
        return key.Trim().ToLowerInvariant() switch
        {
            "art" => ExperienceCategoryKind.ArtCulture,
            "nature" => ExperienceCategoryKind.NatureEscapes,
            "history" => ExperienceCategoryKind.HistoricalSites,
            "culinary" => ExperienceCategoryKind.CulinaryJourneys,
            _ => null
        };
    }

    public static RegionKind? ParseRegion(string? key)
    {
        if (string.IsNullOrWhiteSpace(key)) return null;
        return key.Trim().ToLowerInvariant() switch
        {
            "europe" => RegionKind.Europe,
            "asia" => RegionKind.Asia,
            "americas" => RegionKind.NorthAmerica,
            "north-america" => RegionKind.NorthAmerica,
            "northamerica" => RegionKind.NorthAmerica,
            "south-america" => RegionKind.SouthAmerica,
            "southamerica" => RegionKind.SouthAmerica,
            "africa" => RegionKind.Africa,
            "middleeast" => RegionKind.MiddleEast,
            "middle-east" => RegionKind.MiddleEast,
            "oceania" => RegionKind.Oceania,
            "pacific-islands" => RegionKind.PacificIslands,
            "pacificislands" => RegionKind.PacificIslands,
            _ => null
        };
    }

    public static string CategoryFilterValue(ExperienceCategoryKind c) => c switch
    {
        ExperienceCategoryKind.ArtCulture => "art",
        ExperienceCategoryKind.NatureEscapes => "nature",
        ExperienceCategoryKind.HistoricalSites => "history",
        ExperienceCategoryKind.CulinaryJourneys => "culinary",
        _ => ""
    };

    public static string RegionFilterValue(RegionKind r) => r switch
    {
        RegionKind.Europe => "europe",
        RegionKind.Asia => "asia",
        RegionKind.NorthAmerica => "north-america",
        RegionKind.Africa => "africa",
        RegionKind.MiddleEast => "middleeast",
        RegionKind.SouthAmerica => "south-america",
        RegionKind.Oceania => "oceania",
        RegionKind.PacificIslands => "pacific-islands",
        _ => ""
    };

    public static string CategoryDisplay(ExperienceCategoryKind c) => c switch
    {
        ExperienceCategoryKind.ArtCulture => "Art & Culture",
        ExperienceCategoryKind.NatureEscapes => "Nature Escapes",
        ExperienceCategoryKind.HistoricalSites => "Historical Sites",
        ExperienceCategoryKind.CulinaryJourneys => "Culinary Journeys",
        _ => ""
    };

    public static string RegionDisplay(RegionKind r) => r switch
    {
        RegionKind.Europe => "Europe",
        RegionKind.Asia => "Asia",
        RegionKind.NorthAmerica => "North America",
        RegionKind.Africa => "Africa",
        RegionKind.MiddleEast => "Middle East",
        RegionKind.SouthAmerica => "South America",
        RegionKind.Oceania => "Oceania",
        RegionKind.PacificIslands => "Pacific Islands",
        _ => ""
    };

    /// <summary>Parses price tier: any, budget, mid, luxury</summary>
    public static string? ParsePriceTier(string? key)
    {
        if (string.IsNullOrWhiteSpace(key) || key.Equals("any", StringComparison.OrdinalIgnoreCase))
            return null;
        var k = key.Trim().ToLowerInvariant();
        return k is "budget" or "mid" or "luxury" ? k : null;
    }

    /// <summary>Parses stars filter: any, 3, 4, 5</summary>
    public static int? ParseStars(string? key)
    {
        if (string.IsNullOrWhiteSpace(key) || key.Equals("any", StringComparison.OrdinalIgnoreCase))
            return null;
        if (int.TryParse(key.Trim(), out var n) && n is >= 3 and <= 5)
            return n;
        return null;
    }

    /// <summary>wifi, pool, spa, gym — matches AmenitiesCsv contains.</summary>
    public static string? ParseAmenity(string? key)
    {
        if (string.IsNullOrWhiteSpace(key) || key.Equals("any", StringComparison.OrdinalIgnoreCase))
            return null;
        return key.Trim().ToLowerInvariant();
    }
}
