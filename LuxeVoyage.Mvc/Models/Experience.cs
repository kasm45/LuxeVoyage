using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models;

public class Experience
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

    [MaxLength(600)]
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

    [MaxLength(200)]
    public string? TagLine { get; set; }

    /// <summary>When false, hidden from public catalog and detail pages.</summary>
    public bool IsActive { get; set; } = true;

    // Listing card overrides
    [MaxLength(120)]
    public string? CardTitle { get; set; }
    [MaxLength(400)]
    public string? CardSummary { get; set; }
    [MaxLength(400)]
    public string? CardImageUrl { get; set; }
    [MaxLength(80)]
    public string? CardBadge { get; set; }
    [MaxLength(80)]
    public string? CardRegion { get; set; }
    [MaxLength(60)]
    public string? CardPriceHint { get; set; }
    public double? CardRating { get; set; }
    public bool IsVisibleOnListing { get; set; } = true;

    // Detail page overrides
    [MaxLength(120)]
    public string? HeroTitle { get; set; }
    [MaxLength(220)]
    public string? HeroSubtitle { get; set; }
    [MaxLength(400)]
    public string? HeroImageUrl { get; set; }
    public string? DetailDescription { get; set; }
    [MaxLength(220)]
    public string? DetailTagline { get; set; }
    [MaxLength(80)]
    public string? DurationText { get; set; }
    [MaxLength(80)]
    public string? GroupSizeText { get; set; }
    [MaxLength(160)]
    public string? MeetingPoint { get; set; }
    [MaxLength(4000)]
    public string? IncludedItemsCsv { get; set; }
    [MaxLength(4000)]
    public string? HighlightsCsv { get; set; }
    [MaxLength(6000)]
    public string? ItineraryText { get; set; }
    [MaxLength(400)]
    public string? GalleryImage1Url { get; set; }
    [MaxLength(400)]
    public string? GalleryImage2Url { get; set; }
    [MaxLength(400)]
    public string? GalleryImage3Url { get; set; }
    [MaxLength(400)]
    public string? GalleryImage4Url { get; set; }
}
