using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models;

public class Stay
{
    public int Id { get; set; }

    [Required, MaxLength(160)]
    public string Name { get; set; } = "";

    [Required, MaxLength(180)]
    public string Slug { get; set; } = "";

    [MaxLength(80)]
    public string? StayType { get; set; }

    public decimal PricePerNight { get; set; }

    /// <summary>1–5 hotel star level used for filtering.</summary>
    public int StarRating { get; set; }

    public RegionKind Region { get; set; }

    [MaxLength(120)]
    public string CityLine { get; set; } = "";

    [MaxLength(400)]
    public string? ImageUrl { get; set; }

    /// <summary>Comma-separated canonical keys: wifi, pool, gym, spa, dining, etc.</summary>
    [MaxLength(400)]
    public string AmenitiesCsv { get; set; } = "";

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
    [MaxLength(220)]
    public string? AddressLabel { get; set; }
    [MaxLength(120)]
    public string? RoomType { get; set; }
    [MaxLength(80)]
    public string? GuestCapacity { get; set; }
    [MaxLength(120)]
    public string? BedInfo { get; set; }
    [MaxLength(4000)]
    public string? AmenitiesDetailCsv { get; set; }
    [MaxLength(4000)]
    public string? HighlightsCsv { get; set; }
    [MaxLength(4000)]
    public string? NearbyAttractionsCsv { get; set; }
    [MaxLength(2000)]
    public string? CheckInInfo { get; set; }
    [MaxLength(2000)]
    public string? CancellationPolicy { get; set; }
    [MaxLength(400)]
    public string? GalleryImage1Url { get; set; }
    [MaxLength(400)]
    public string? GalleryImage2Url { get; set; }
    [MaxLength(400)]
    public string? GalleryImage3Url { get; set; }
    [MaxLength(400)]
    public string? GalleryImage4Url { get; set; }
}
