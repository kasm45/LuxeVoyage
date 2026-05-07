namespace LuxeVoyage.Mvc.Models.ViewModels;

public class ToursIndexViewModel
{
    public IReadOnlyList<TourCardVm> Tours { get; set; } = Array.Empty<TourCardVm>();
    public string Sort { get; set; } = "popularity";
    public int Page { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
}

public class TourCardVm
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Slug { get; set; } = "";
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public string CategoryLabel { get; set; } = "";
    public string? ImageUrl { get; set; }
    public string? Summary { get; set; }
    public double Rating { get; set; }
    public int ReviewCount { get; set; }
    public bool IsFavorite { get; set; }
    public string? PriceHint { get; set; }
    public string? RegionDisplay { get; set; }
}
