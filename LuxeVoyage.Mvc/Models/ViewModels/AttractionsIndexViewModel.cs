namespace LuxeVoyage.Mvc.Models.ViewModels;

public class AttractionsIndexViewModel
{
    public string Headline { get; set; } = "Discover Extraordinary Attractions";
    public string Subtitle { get; set; } = "";
    public string SourceController { get; set; } = "Experiences";
    public string SourceArea { get; set; } = "experiences";

    public IReadOnlyList<AttractionCardVm> Items { get; set; } = Array.Empty<AttractionCardVm>();

    public string? Category { get; set; }
    public string? Region { get; set; }
    public bool ExpandFilters { get; set; }

    public int TotalFilteredCount { get; set; }

    /// <summary>experience | destination — controls favorite toggle target.</summary>
    public string CardKind { get; set; } = "experience";
}

public class AttractionCardVm
{
    public int Id { get; set; }
    public string Slug { get; set; } = "";
    public string Title { get; set; } = "";
    public string CategoryLabel { get; set; } = "";
    public string RegionDisplay { get; set; } = "";
    public string? ImageUrl { get; set; }
    public double? Rating { get; set; }
    public string? Summary { get; set; }
    public bool IsFavorite { get; set; }

    /// <summary>Listing-card badge override (destinations); falls back to <see cref="CategoryLabel"/>.</summary>
    public string? ListingBadge { get; set; }

    /// <summary>Optional “from $…” line on destination listing cards.</summary>
    public string? ListingPriceHint { get; set; }
}
