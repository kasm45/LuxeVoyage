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
}
