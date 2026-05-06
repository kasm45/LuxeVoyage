namespace LuxeVoyage.Mvc.Models.ViewModels;

public class HotelsIndexViewModel
{
    public IReadOnlyList<StayCardVm> Stays { get; set; } = Array.Empty<StayCardVm>();

    public string PriceFilter { get; set; } = "any";
    public string StarsFilter { get; set; } = "any";
    public string AmenityFilter { get; set; } = "any";

    public string ViewMode { get; set; } = "grid";

    public int TotalCount { get; set; }

    /// <summary>wifi, pool, spa for dropdown options.</summary>
    public IReadOnlyList<string> AmenityOptions { get; set; } =
        new[] { "wifi", "pool", "spa", "gym", "dining" };
}

public class StayCardVm
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Slug { get; set; } = "";
    public decimal PricePerNight { get; set; }
    public int StarRating { get; set; }
    public double StarDisplay { get; set; }
    public string CityLine { get; set; } = "";
    public string? ImageUrl { get; set; }
    public IReadOnlyList<string> AmenityTags { get; set; } = Array.Empty<string>();
    public bool IsFavorite { get; set; }
}
