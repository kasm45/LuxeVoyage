namespace LuxeVoyage.Mvc.Models.ViewModels;

public class SearchResultsViewModel
{
    public string? Query { get; set; }
    public int? DestinationId { get; set; }
    public string? DateParam { get; set; }
    public string? DateEndParam { get; set; }

    /// <summary>Human-readable date summary for the results header.</summary>
    public string? TravelDatesSummary { get; set; }

    public IReadOnlyList<SearchHitVm> Destinations { get; set; } = Array.Empty<SearchHitVm>();
    public IReadOnlyList<SearchHitVm> Experiences { get; set; } = Array.Empty<SearchHitVm>();
    public IReadOnlyList<SearchHitVm> Tours { get; set; } = Array.Empty<SearchHitVm>();
    public IReadOnlyList<SearchHitVm> Stays { get; set; } = Array.Empty<SearchHitVm>();

    public bool HasAnyResults =>
        Destinations.Count + Experiences.Count + Tours.Count + Stays.Count > 0;
}

public class SearchHitVm
{
    public string Title { get; set; } = "";
    public string? Subtitle { get; set; }
    public string? ImageUrl { get; set; }
    public string DetailUrl { get; set; } = "";
}
