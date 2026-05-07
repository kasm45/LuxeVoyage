using LuxeVoyage.Mvc.Helpers;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;

namespace LuxeVoyage.Mvc.Mapping;

public static class CatalogDisplayMapper
{
    public static AttractionCardVm ToExperienceCard(Experience e, bool isFavorite) => new()
    {
        Id = e.Id,
        Slug = e.Slug,
        Title = string.IsNullOrWhiteSpace(e.CardTitle) ? e.Title : e.CardTitle.Trim(),
        CategoryLabel = CatalogQueryHelper.CategoryDisplay(e.Category),
        RegionDisplay = string.IsNullOrWhiteSpace(e.CardRegion) ? CatalogQueryHelper.RegionDisplay(e.Region) : e.CardRegion.Trim(),
        ImageUrl = string.IsNullOrWhiteSpace(e.CardImageUrl) ? e.ImageUrl : e.CardImageUrl.Trim(),
        Rating = e.CardRating ?? e.Rating,
        Summary = string.IsNullOrWhiteSpace(e.CardSummary) ? e.Summary : e.CardSummary.Trim(),
        IsFavorite = isFavorite,
        ListingBadge = string.IsNullOrWhiteSpace(e.CardBadge) ? null : e.CardBadge.Trim(),
        ListingPriceHint = string.IsNullOrWhiteSpace(e.CardPriceHint) ? e.PriceHint : e.CardPriceHint.Trim()
    };

    public static ExperienceDetailViewModel ToExperienceDetail(Experience e) => new()
    {
        Id = e.Slug,
        NumericId = e.Id,
        Title = string.IsNullOrWhiteSpace(e.HeroTitle) ? e.Title : e.HeroTitle.Trim(),
        HeroSubtitle = e.HeroSubtitle,
        BreadcrumbRegion = e.BreadcrumbRegion ?? CatalogQueryHelper.RegionDisplay(e.Region),
        BreadcrumbCity = e.BreadcrumbCity ?? "",
        BreadcrumbCurrent = e.BreadcrumbCurrent ?? e.Title,
        Location = e.MeetingPoint ?? e.LocationLabel ?? "",
        PriceDisplay = e.CardPriceHint ?? e.PriceHint ?? "",
        Summary = e.Summary,
        Rating = e.CardRating ?? e.Rating,
        DetailKind = "experience",
        HeroImageUrl = FirstNonEmpty(e.HeroImageUrl, e.GalleryImage1Url, e.CardImageUrl, e.ImageUrl),
        GalleryImage1Url = FirstNonEmpty(e.GalleryImage1Url, e.HeroImageUrl, e.CardImageUrl, e.ImageUrl),
        GalleryImage2Url = e.GalleryImage2Url,
        GalleryImage3Url = e.GalleryImage3Url,
        GalleryImage4Url = e.GalleryImage4Url,
        DetailDescription = string.IsNullOrWhiteSpace(e.DetailDescription) ? e.Summary : e.DetailDescription,
        DetailTagline = string.IsNullOrWhiteSpace(e.DetailTagline) ? e.TagLine : e.DetailTagline,
        DurationDisplay = e.DurationText,
        GroupSizeDisplay = e.GroupSizeText,
        IncludedItemsText = e.IncludedItemsCsv,
        HighlightsText = string.IsNullOrWhiteSpace(e.HighlightsCsv) ? e.TagLine : e.HighlightsCsv,
        ItineraryText = e.ItineraryText
    };

    public static TourCardVm ToTourCard(Tour t, bool isFavorite) => new()
    {
        Id = t.Id,
        Title = string.IsNullOrWhiteSpace(t.CardTitle) ? t.Title : t.CardTitle.Trim(),
        Slug = t.Slug,
        Price = t.Price,
        DurationDays = t.DurationDays,
        CategoryLabel = string.IsNullOrWhiteSpace(t.CardBadge) ? t.CategoryLabel : t.CardBadge.Trim(),
        ImageUrl = string.IsNullOrWhiteSpace(t.CardImageUrl) ? t.ImageUrl : t.CardImageUrl.Trim(),
        Summary = string.IsNullOrWhiteSpace(t.CardSummary) ? t.Summary : t.CardSummary.Trim(),
        Rating = t.CardRating ?? t.Rating,
        ReviewCount = t.ReviewCount,
        IsFavorite = isFavorite,
        PriceHint = string.IsNullOrWhiteSpace(t.CardPriceHint) ? null : t.CardPriceHint.Trim(),
        RegionDisplay = string.IsNullOrWhiteSpace(t.CardRegion) ? CatalogQueryHelper.RegionDisplay(t.Region) : t.CardRegion.Trim()
    };

    public static ExperienceDetailViewModel ToTourDetail(Tour t) => new()
    {
        Id = t.Slug,
        NumericId = t.Id,
        Title = string.IsNullOrWhiteSpace(t.HeroTitle) ? t.Title : t.HeroTitle.Trim(),
        HeroSubtitle = t.HeroSubtitle,
        BreadcrumbRegion = "Tours",
        BreadcrumbCity = string.IsNullOrWhiteSpace(t.DestinationLabel) ? CatalogQueryHelper.RegionDisplay(t.Region) : t.DestinationLabel,
        BreadcrumbCurrent = t.Title,
        Location = t.DestinationLabel ?? CatalogQueryHelper.RegionDisplay(t.Region),
        PriceDisplay = string.IsNullOrWhiteSpace(t.CardPriceHint) ? $"${t.Price:0}" : t.CardPriceHint,
        Summary = t.Summary,
        Rating = t.CardRating ?? t.Rating,
        DetailKind = "tour",
        HeroImageUrl = FirstNonEmpty(t.HeroImageUrl, t.GalleryImage1Url, t.CardImageUrl, t.ImageUrl),
        DurationDays = t.DurationDays,
        ReviewCount = t.ReviewCount,
        GalleryImage1Url = FirstNonEmpty(t.GalleryImage1Url, t.HeroImageUrl, t.CardImageUrl, t.ImageUrl),
        GalleryImage2Url = t.GalleryImage2Url,
        GalleryImage3Url = t.GalleryImage3Url,
        GalleryImage4Url = t.GalleryImage4Url,
        DetailDescription = string.IsNullOrWhiteSpace(t.DetailDescription) ? t.Summary : t.DetailDescription,
        DetailTagline = t.DetailTagline,
        DurationDisplay = $"{t.DurationDays} Days",
        GroupSizeDisplay = t.GroupSizeText,
        IncludedItemsText = t.IncludedItemsCsv,
        HighlightsText = t.HighlightsCsv,
        ItineraryText = t.ItineraryText,
        WhatToBringText = t.WhatToBring,
        CancellationPolicyText = t.CancellationPolicy
    };

    public static StayCardVm ToStayCard(Stay s, bool isFavorite) => new()
    {
        Id = s.Id,
        Name = string.IsNullOrWhiteSpace(s.CardTitle) ? s.Name : s.CardTitle.Trim(),
        Slug = s.Slug,
        PricePerNight = s.PricePerNight,
        StarRating = s.StarRating,
        StarDisplay = s.CardRating ?? s.StarRating,
        CityLine = string.IsNullOrWhiteSpace(s.CardRegion) ? s.CityLine : s.CardRegion.Trim(),
        ImageUrl = string.IsNullOrWhiteSpace(s.CardImageUrl) ? s.ImageUrl : s.CardImageUrl.Trim(),
        AmenityTags = CatalogTextHelper.SplitList(string.IsNullOrWhiteSpace(s.AmenitiesDetailCsv) ? s.AmenitiesCsv : s.AmenitiesDetailCsv),
        IsFavorite = isFavorite,
        Summary = string.IsNullOrWhiteSpace(s.CardSummary) ? null : s.CardSummary.Trim(),
        Badge = string.IsNullOrWhiteSpace(s.CardBadge) ? s.StayType : s.CardBadge,
        PriceHint = string.IsNullOrWhiteSpace(s.CardPriceHint) ? null : s.CardPriceHint.Trim()
    };

    public static ExperienceDetailViewModel ToStayDetail(Stay s) => new()
    {
        Id = s.Slug,
        NumericId = s.Id,
        Title = string.IsNullOrWhiteSpace(s.HeroTitle) ? s.Name : s.HeroTitle.Trim(),
        HeroSubtitle = s.HeroSubtitle,
        BreadcrumbRegion = CatalogQueryHelper.RegionDisplay(s.Region),
        BreadcrumbCity = s.CityLine ?? "",
        BreadcrumbCurrent = s.Name,
        Location = s.AddressLabel ?? s.CityLine ?? "",
        PriceDisplay = string.IsNullOrWhiteSpace(s.CardPriceHint) ? $"${s.PricePerNight:0}" : s.CardPriceHint,
        Summary = string.IsNullOrWhiteSpace(s.CardSummary) ? null : s.CardSummary,
        Rating = s.CardRating ?? s.StarRating,
        DetailKind = "stay",
        HeroImageUrl = FirstNonEmpty(s.HeroImageUrl, s.GalleryImage1Url, s.CardImageUrl, s.ImageUrl),
        GalleryImage1Url = FirstNonEmpty(s.GalleryImage1Url, s.HeroImageUrl, s.CardImageUrl, s.ImageUrl),
        GalleryImage2Url = s.GalleryImage2Url,
        GalleryImage3Url = s.GalleryImage3Url,
        GalleryImage4Url = s.GalleryImage4Url,
        DetailDescription = s.DetailDescription,
        DetailTagline = s.DetailTagline,
        GroupSizeDisplay = s.GuestCapacity,
        IncludedItemsText = string.IsNullOrWhiteSpace(s.AmenitiesDetailCsv) ? s.AmenitiesCsv : s.AmenitiesDetailCsv,
        HighlightsText = s.HighlightsCsv,
        NearbyAttractionsText = s.NearbyAttractionsCsv,
        CheckInInfoText = s.CheckInInfo,
        CancellationPolicyText = s.CancellationPolicy,
        RoomTypeText = s.RoomType,
        BedInfoText = s.BedInfo
    };

    private static string? FirstNonEmpty(params string?[] values) => values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));
}
