using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models;

public class Destination
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Title { get; set; } = "";

    [Required, MaxLength(160)]
    public string Slug { get; set; } = "";

    public ExperienceCategoryKind Category { get; set; }
    public RegionKind Region { get; set; }

    [MaxLength(400)]
    public string? ImageUrl { get; set; }

    [MaxLength(300)]
    public string? Summary { get; set; }

    [MaxLength(120)]
    public string? LocationLabel { get; set; }

    public double? Rating { get; set; }

    [MaxLength(60)]
    public string? PriceHint { get; set; }

    [MaxLength(80)]
    public string? BreadcrumbRegion { get; set; }

    [MaxLength(80)]
    public string? BreadcrumbCity { get; set; }

    [MaxLength(80)]
    public string? BreadcrumbCurrent { get; set; }

    /// <summary>Comma-separated amenity-style tags for display chips.</summary>
    [MaxLength(200)]
    public string? TagLine { get; set; }

    /// <summary>Optional hero image for the destination detail page (falls back to <see cref="ImageUrl"/>).</summary>
    [MaxLength(400)]
    public string? HeroImageUrl { get; set; }

    /// <summary>Rich body copy for the detail page (Markdown not supported; plain text/HTML-safe paragraphs).</summary>
    public string? LongDescription { get; set; }

    [MaxLength(500)]
    public string? BestTimeToVisit { get; set; }

    [MaxLength(2000)]
    public string? WeatherClimate { get; set; }

    /// <summary>Copy for the “Where you'll be” section (routes, districts, meeting notes).</summary>
    [MaxLength(4000)]
    public string? WhereYoullBeText { get; set; }

    /// <summary>Optional stylized map image for the location section.</summary>
    [MaxLength(600)]
    public string? MapImageUrl { get; set; }

    /// <summary>Comma- or newline-separated extra gallery image URLs (beyond hero/card).</summary>
    [MaxLength(4000)]
    public string? GalleryImagesCsv { get; set; }

    /// <summary>Comma- or newline-separated highlights / included experiences titles.</summary>
    [MaxLength(4000)]
    public string? HighlightsCsv { get; set; }

    /// <summary>When false, hidden from public catalog and detail returns not-found.</summary>
    public bool IsActive { get; set; } = true;

    // ----- Listing page only (/destinations grid) -----
    [MaxLength(120)]
    public string? CardTitle { get; set; }

    [MaxLength(400)]
    public string? CardSummary { get; set; }

    [MaxLength(400)]
    public string? CardImageUrl { get; set; }

    [MaxLength(80)]
    public string? CardBadge { get; set; }

    /// <summary>Optional override for the region line on listing cards; falls back to <see cref="Region"/> display.</summary>
    [MaxLength(80)]
    public string? CardRegion { get; set; }

    [MaxLength(60)]
    public string? CardPriceHint { get; set; }

    public double? CardRating { get; set; }

    /// <summary>When false, excluded from /destinations listing (detail may still work when <see cref="IsActive"/>).</summary>
    public bool IsVisibleOnListing { get; set; } = true;

    // ----- Detail page only (/destinations/{slug}) -----
    [MaxLength(120)]
    public string? HeroTitle { get; set; }

    [MaxLength(200)]
    public string? HeroSubtitle { get; set; }

    /// <summary>Primary long body for the destination detail page.</summary>
    public string? DetailDescription { get; set; }

    [MaxLength(200)]
    public string? DetailTagline { get; set; }

    [MaxLength(400)]
    public string? GalleryImage1Url { get; set; }

    [MaxLength(400)]
    public string? GalleryImage2Url { get; set; }

    [MaxLength(400)]
    public string? GalleryImage3Url { get; set; }

    [MaxLength(400)]
    public string? GalleryImage4Url { get; set; }
}
