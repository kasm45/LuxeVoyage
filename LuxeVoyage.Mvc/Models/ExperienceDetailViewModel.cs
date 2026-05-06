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
    /// <summary>experience | destination | stay — drives booking query params.</summary>
    public string DetailKind { get; set; } = "experience";
}
