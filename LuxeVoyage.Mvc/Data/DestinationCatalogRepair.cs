using LuxeVoyage.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Data;

/// <summary>
/// Aligns canonical catalog destinations (slug + CMS fields + activation) after seed inserts,
/// without creating duplicate rows when titles change over time.
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

            CopyTemplateOntoRow(template, row);
        }

        await ctx.SaveChangesAsync();
    }

    private static void CopyTemplateOntoRow(Destination src, Destination dest)
    {
        dest.Title = src.Title;
        dest.Slug = src.Slug;
        dest.Category = src.Category;
        dest.Region = src.Region;
        if (string.IsNullOrWhiteSpace(dest.ImageUrl))
            dest.ImageUrl = src.ImageUrl;
        dest.Summary = src.Summary;
        dest.LocationLabel = src.LocationLabel;
        dest.Rating = src.Rating;
        dest.PriceHint = src.PriceHint;
        dest.BreadcrumbRegion = src.BreadcrumbRegion;
        dest.BreadcrumbCity = src.BreadcrumbCity;
        dest.BreadcrumbCurrent = src.BreadcrumbCurrent;
        dest.TagLine = src.TagLine;
        if (string.IsNullOrWhiteSpace(dest.HeroImageUrl))
            dest.HeroImageUrl = src.HeroImageUrl;
        dest.LongDescription = src.LongDescription;
        dest.BestTimeToVisit = src.BestTimeToVisit;
        dest.WeatherClimate = src.WeatherClimate;
        dest.WhereYoullBeText = src.WhereYoullBeText;
        if (string.IsNullOrWhiteSpace(dest.MapImageUrl))
            dest.MapImageUrl = src.MapImageUrl;
        if (string.IsNullOrWhiteSpace(dest.GalleryImagesCsv))
            dest.GalleryImagesCsv = src.GalleryImagesCsv;
        dest.HighlightsCsv = src.HighlightsCsv;
        dest.IsActive = src.IsActive;
        dest.CardTitle = src.CardTitle;
        dest.CardSummary = src.CardSummary;
        if (string.IsNullOrWhiteSpace(dest.CardImageUrl))
            dest.CardImageUrl = src.CardImageUrl;
        dest.CardBadge = src.CardBadge;
        dest.CardRegion = src.CardRegion;
        dest.CardPriceHint = src.CardPriceHint;
        dest.CardRating = src.CardRating;
        dest.IsVisibleOnListing = src.IsVisibleOnListing;
        dest.HeroTitle = src.HeroTitle;
        dest.HeroSubtitle = src.HeroSubtitle;
        dest.DetailDescription = src.DetailDescription;
        dest.DetailTagline = src.DetailTagline;
        if (string.IsNullOrWhiteSpace(dest.GalleryImage1Url))
            dest.GalleryImage1Url = src.GalleryImage1Url;
        if (string.IsNullOrWhiteSpace(dest.GalleryImage2Url))
            dest.GalleryImage2Url = src.GalleryImage2Url;
        if (string.IsNullOrWhiteSpace(dest.GalleryImage3Url))
            dest.GalleryImage3Url = src.GalleryImage3Url;
        if (string.IsNullOrWhiteSpace(dest.GalleryImage4Url))
            dest.GalleryImage4Url = src.GalleryImage4Url;
    }

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
