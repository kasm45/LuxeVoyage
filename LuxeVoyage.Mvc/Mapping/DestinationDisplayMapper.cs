using LuxeVoyage.Mvc.Helpers;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;

namespace LuxeVoyage.Mvc.Mapping;

public static class DestinationDisplayMapper
{
    public static string EffectiveCardTitle(Destination d) =>
        string.IsNullOrWhiteSpace(d.CardTitle) ? d.Title : d.CardTitle.Trim();

    public static string? EffectiveCardSummary(Destination d) =>
        string.IsNullOrWhiteSpace(d.CardSummary) ? d.Summary : d.CardSummary.Trim();

    public static string? EffectiveCardImageUrl(Destination d) =>
        string.IsNullOrWhiteSpace(d.CardImageUrl) ? d.ImageUrl : d.CardImageUrl.Trim();

    public static string? EffectiveCardBadge(Destination d) => d.CardBadge?.Trim();

    public static string EffectiveCardRegionDisplay(Destination d) =>
        !string.IsNullOrWhiteSpace(d.CardRegion)
            ? d.CardRegion.Trim()
            : CatalogQueryHelper.RegionDisplay(d.Region);

    public static string? EffectiveCardPriceHint(Destination d) =>
        string.IsNullOrWhiteSpace(d.CardPriceHint) ? d.PriceHint : d.CardPriceHint.Trim();

    public static double? EffectiveCardRating(Destination d) => d.CardRating ?? d.Rating;

    public static string EffectiveHeroTitle(Destination d) =>
        string.IsNullOrWhiteSpace(d.HeroTitle) ? d.Title : d.HeroTitle.Trim();

    public static string? EffectiveHeroSubtitle(Destination d)
    {
        var h = d.HeroSubtitle?.Trim();
        return string.IsNullOrEmpty(h) ? null : h;
    }

    public static string? EffectiveDetailBody(Destination d)
    {
        if (!string.IsNullOrWhiteSpace(d.DetailDescription))
            return d.DetailDescription;
        return d.LongDescription;
    }

    public static string? EffectiveDetailTagline(Destination d) =>
        string.IsNullOrWhiteSpace(d.DetailTagline) ? d.TagLine : d.DetailTagline.Trim();

    /// <summary>Resolved URL for a gallery slot; uses legacy hero/csv only when the explicit field is empty (never duplicates another slot’s image).</summary>
    public static string? ResolvedGalleryUrl(Destination d, int slot)
    {
        if (slot is < 1 or > 4)
            return null;

        var explicitUrl = slot switch
        {
            1 => d.GalleryImage1Url,
            2 => d.GalleryImage2Url,
            3 => d.GalleryImage3Url,
            4 => d.GalleryImage4Url,
            _ => null
        };
        if (!string.IsNullOrWhiteSpace(explicitUrl))
            return explicitUrl.Trim();

        var extras = CatalogTextHelper.SplitList(d.GalleryImagesCsv);
        return slot switch
        {
            1 => FirstNonEmpty(d.HeroImageUrl, d.ImageUrl),
            2 => extras.Count > 0 ? extras[0] : null,
            3 => extras.Count > 1 ? extras[1] : null,
            4 => extras.Count > 2 ? extras[2] : null,
            _ => null
        };
    }

    private static string? FirstNonEmpty(params string?[] candidates)
    {
        foreach (var c in candidates)
        {
            if (!string.IsNullOrWhiteSpace(c))
                return c.Trim();
        }

        return null;
    }

    public static AttractionCardVm ToListingCard(Destination d, bool isFavorite) =>
        new()
        {
            Id = d.Id,
            Slug = d.Slug,
            Title = EffectiveCardTitle(d),
            CategoryLabel = CatalogQueryHelper.CategoryDisplay(d.Category),
            RegionDisplay = EffectiveCardRegionDisplay(d),
            ImageUrl = EffectiveCardImageUrl(d),
            Rating = EffectiveCardRating(d),
            Summary = EffectiveCardSummary(d),
            IsFavorite = isFavorite,
            ListingBadge = EffectiveCardBadge(d),
            ListingPriceHint = EffectiveCardPriceHint(d)
        };

    public static ExperienceDetailViewModel ToDetail(Destination d)
    {
        var highlights = CatalogTextHelper.SplitList(d.HighlightsCsv);
        if (highlights.Count == 0 && !string.IsNullOrWhiteSpace(d.TagLine))
            highlights = CatalogTextHelper.SplitList(d.TagLine.Replace('|', ','));

        var body = EffectiveDetailBody(d);
        var legacySummary = d.Summary?.Trim();
        var secondarySummary = string.IsNullOrWhiteSpace(body) ? legacySummary : null;

        var g1 = ResolvedGalleryUrl(d, 1);
        var g2 = ResolvedGalleryUrl(d, 2);
        var g3 = ResolvedGalleryUrl(d, 3);
        var g4 = ResolvedGalleryUrl(d, 4);

        return new ExperienceDetailViewModel
        {
            Id = d.Slug,
            NumericId = d.Id,
            Title = EffectiveHeroTitle(d),
            HeroSubtitle = EffectiveHeroSubtitle(d),
            BreadcrumbRegion = d.BreadcrumbRegion ?? CatalogQueryHelper.RegionDisplay(d.Region),
            BreadcrumbCity = d.BreadcrumbCity ?? "",
            BreadcrumbCurrent = d.BreadcrumbCurrent ?? EffectiveHeroTitle(d),
            Location = d.LocationLabel ?? "",
            PriceDisplay = d.PriceHint ?? "",
            Summary = secondarySummary,
            Rating = d.Rating,
            DetailKind = "destination",
            HeroImageUrl = g1,
            LongDescription = body,
            BestTimeToVisit = d.BestTimeToVisit,
            WeatherClimate = d.WeatherClimate,
            WhereYoullBeText = d.WhereYoullBeText,
            MapImageUrl = d.MapImageUrl,
            GalleryImageUrls = Array.Empty<string>(),
            GalleryImage1Url = g1,
            GalleryImage2Url = g2,
            GalleryImage3Url = g3,
            GalleryImage4Url = g4,
            HighlightLines = highlights,
            HighlightTagline = EffectiveDetailTagline(d)
        };
    }
}
