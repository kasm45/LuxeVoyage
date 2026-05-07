namespace LuxeVoyage.Mvc.Models;

/// <summary>
/// Mock detail payload for destination/experience detail pages (LuxeVoyage detail template).
/// </summary>
public class ExperienceDetailViewModel
{
    public string Id { get; set; } = "";
    public int? NumericId { get; set; }
    public string Title { get; set; } = "";
    public string BreadcrumbRegion { get; set; } = "";
    public string BreadcrumbCity { get; set; } = "";
    public string BreadcrumbCurrent { get; set; } = "";
    public string Location { get; set; } = "";
    public string PriceDisplay { get; set; } = "";
    public string? Summary { get; set; }
    public double? Rating { get; set; }
    /// <summary>experience | destination | stay | tour — drives booking query params.</summary>
    public string DetailKind { get; set; } = "experience";

    /// <summary>Optional hero image for catalog detail (tour/stay) when the template should show real imagery.</summary>
    public string? HeroImageUrl { get; set; }

    /// <summary>Tour length in days when <see cref="DetailKind"/> is tour.</summary>
    public int? DurationDays { get; set; }

    /// <summary>Review count when shown on detail header (e.g. tours).</summary>
    public int? ReviewCount { get; set; }

    // ----- Destination detail (public /destinations/{slug}) -----
    public string? LongDescription { get; set; }
    public string? BestTimeToVisit { get; set; }
    public string? WeatherClimate { get; set; }
    public string? WhereYoullBeText { get; set; }
    public string? MapImageUrl { get; set; }
    public IReadOnlyList<string> GalleryImageUrls { get; set; } = Array.Empty<string>();

    /// <summary>Explicit gallery slots for destination detail (resolved URLs including legacy fallback when empty).</summary>
    public string? GalleryImage1Url { get; set; }
    public string? GalleryImage2Url { get; set; }
    public string? GalleryImage3Url { get; set; }
    public string? GalleryImage4Url { get; set; }

    public IReadOnlyList<string> HighlightLines { get; set; } = Array.Empty<string>();
    public string? HighlightTagline { get; set; }

    /// <summary>Optional subtitle under the H1 on destination detail.</summary>
    public string? HeroSubtitle { get; set; }

    public string? DetailDescription { get; set; }
    public string? DetailTagline { get; set; }
    public string? DurationDisplay { get; set; }
    public string? GroupSizeDisplay { get; set; }
    public string? IncludedItemsText { get; set; }
    public string? HighlightsText { get; set; }
    public string? ItineraryText { get; set; }
    public string? WhatToBringText { get; set; }
    public string? CancellationPolicyText { get; set; }
    public string? NearbyAttractionsText { get; set; }
    public string? CheckInInfoText { get; set; }
    public string? BedInfoText { get; set; }
    public string? RoomTypeText { get; set; }

    /// <summary>Destination-specific curated attractions shown on detail pages.</summary>
    public IReadOnlyList<DestinationAttractionViewModel> TopAttractions { get; set; } = Array.Empty<DestinationAttractionViewModel>();
}

public class DestinationAttractionViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string City { get; set; } = "";
    public string Category { get; set; } = "";
    public string? ImageUrl { get; set; }
}
