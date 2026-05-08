using LuxeVoyage.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Data;

/// <summary>
/// Keeps canonical catalog destinations present for fixed slugs after seed runs.
/// New rows receive the full template (<see cref="CloneTemplate"/>).
/// Existing rows use CMS-safe repair: only missing or whitespace-only fields are filled from the template;
/// activation, listing visibility, category/region enums, and any non-empty editor content are left unchanged between startups.
/// </summary>
public static class DestinationCatalogRepair
{
    private static readonly string[] CanonicalSlugs =
    [
        "amalfi-coast",
        "tokyo",
        "zermatt"
    ];

    public static async Task ApplyAsync(ApplicationDbContext ctx)
    {
        foreach (var slug in CanonicalSlugs)
        {
            var template = SeedDestinationsData.All.FirstOrDefault(d => d.Slug == slug);
            if (template == null)
                continue;

            var row = await ctx.Destinations.FirstOrDefaultAsync(d => d.Slug == slug);
            if (row == null)
            {
                ctx.Destinations.Add(CloneTemplate(template));
                continue;
            }

            FillMissingFromTemplate(template, row);
        }

        await ctx.SaveChangesAsync();
    }

    /// <summary>
    /// Merge template into an existing row without overwriting concierge edits.
    /// Strings and URLs: copy only when null or whitespace.
    /// Nullable ratings: copy only when currently null.
    /// Booleans and enums: never updated here so admins control activation, listing visibility, and taxonomy independently of seed templates.
    /// </summary>
    private static void FillMissingFromTemplate(Destination src, Destination dest)
    {
        if (string.IsNullOrWhiteSpace(dest.Title))
            dest.Title = src.Title;

        if (string.IsNullOrWhiteSpace(dest.Slug))
            dest.Slug = src.Slug;

        dest.ImageUrl = CoalesceOptional(dest.ImageUrl, src.ImageUrl);
        dest.Summary = CoalesceOptional(dest.Summary, src.Summary);
        dest.LocationLabel = CoalesceOptional(dest.LocationLabel, src.LocationLabel);

        if (dest.Rating == null)
            dest.Rating = src.Rating;

        dest.PriceHint = CoalesceOptional(dest.PriceHint, src.PriceHint);
        dest.BreadcrumbRegion = CoalesceOptional(dest.BreadcrumbRegion, src.BreadcrumbRegion);
        dest.BreadcrumbCity = CoalesceOptional(dest.BreadcrumbCity, src.BreadcrumbCity);
        dest.BreadcrumbCurrent = CoalesceOptional(dest.BreadcrumbCurrent, src.BreadcrumbCurrent);
        dest.TagLine = CoalesceOptional(dest.TagLine, src.TagLine);
        dest.HeroImageUrl = CoalesceOptional(dest.HeroImageUrl, src.HeroImageUrl);
        dest.LongDescription = CoalesceOptional(dest.LongDescription, src.LongDescription);
        dest.BestTimeToVisit = CoalesceOptional(dest.BestTimeToVisit, src.BestTimeToVisit);
        dest.WeatherClimate = CoalesceOptional(dest.WeatherClimate, src.WeatherClimate);
        dest.WhereYoullBeText = CoalesceOptional(dest.WhereYoullBeText, src.WhereYoullBeText);
        dest.MapImageUrl = CoalesceOptional(dest.MapImageUrl, src.MapImageUrl);
        dest.GalleryImagesCsv = CoalesceOptional(dest.GalleryImagesCsv, src.GalleryImagesCsv);
        dest.HighlightsCsv = CoalesceOptional(dest.HighlightsCsv, src.HighlightsCsv);

        dest.CardTitle = CoalesceOptional(dest.CardTitle, src.CardTitle);
        dest.CardSummary = CoalesceOptional(dest.CardSummary, src.CardSummary);
        dest.CardImageUrl = CoalesceOptional(dest.CardImageUrl, src.CardImageUrl);
        dest.CardBadge = CoalesceOptional(dest.CardBadge, src.CardBadge);
        dest.CardRegion = CoalesceOptional(dest.CardRegion, src.CardRegion);
        dest.CardPriceHint = CoalesceOptional(dest.CardPriceHint, src.CardPriceHint);

        if (dest.CardRating == null)
            dest.CardRating = src.CardRating;

        dest.HeroTitle = CoalesceOptional(dest.HeroTitle, src.HeroTitle);
        dest.HeroSubtitle = CoalesceOptional(dest.HeroSubtitle, src.HeroSubtitle);
        dest.DetailDescription = CoalesceOptional(dest.DetailDescription, src.DetailDescription);
        dest.DetailTagline = CoalesceOptional(dest.DetailTagline, src.DetailTagline);
        dest.GalleryImage1Url = CoalesceOptional(dest.GalleryImage1Url, src.GalleryImage1Url);
        dest.GalleryImage2Url = CoalesceOptional(dest.GalleryImage2Url, src.GalleryImage2Url);
        dest.GalleryImage3Url = CoalesceOptional(dest.GalleryImage3Url, src.GalleryImage3Url);
        dest.GalleryImage4Url = CoalesceOptional(dest.GalleryImage4Url, src.GalleryImage4Url);
    }

    /// <summary>Returns <paramref name="template"/> only when <paramref name="existing"/> is null or whitespace; preserves non-empty CMS values.</summary>
    private static string? CoalesceOptional(string? existing, string? template) =>
        string.IsNullOrWhiteSpace(existing) ? template : existing;

    private static Destination CloneTemplate(Destination t) => new()
    {
        Title = t.Title,
        Slug = t.Slug,
        Category = t.Category,
        Region = t.Region,
        ImageUrl = t.ImageUrl,
        Summary = t.Summary,
        LocationLabel = t.LocationLabel,
        Rating = t.Rating,
        PriceHint = t.PriceHint,
        BreadcrumbRegion = t.BreadcrumbRegion,
        BreadcrumbCity = t.BreadcrumbCity,
        BreadcrumbCurrent = t.BreadcrumbCurrent,
        TagLine = t.TagLine,
        HeroImageUrl = t.HeroImageUrl,
        LongDescription = t.LongDescription,
        BestTimeToVisit = t.BestTimeToVisit,
        WeatherClimate = t.WeatherClimate,
        WhereYoullBeText = t.WhereYoullBeText,
        MapImageUrl = t.MapImageUrl,
        GalleryImagesCsv = t.GalleryImagesCsv,
        HighlightsCsv = t.HighlightsCsv,
        IsActive = t.IsActive,
        CardTitle = t.CardTitle,
        CardSummary = t.CardSummary,
        CardImageUrl = t.CardImageUrl,
        CardBadge = t.CardBadge,
        CardRegion = t.CardRegion,
        CardPriceHint = t.CardPriceHint,
        CardRating = t.CardRating,
        IsVisibleOnListing = t.IsVisibleOnListing,
        HeroTitle = t.HeroTitle,
        HeroSubtitle = t.HeroSubtitle,
        DetailDescription = t.DetailDescription,
        DetailTagline = t.DetailTagline,
        GalleryImage1Url = t.GalleryImage1Url,
        GalleryImage2Url = t.GalleryImage2Url,
        GalleryImage3Url = t.GalleryImage3Url,
        GalleryImage4Url = t.GalleryImage4Url
    };
}
