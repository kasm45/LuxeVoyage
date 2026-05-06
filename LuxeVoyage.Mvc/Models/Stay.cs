using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models;

public class Stay
{
    public int Id { get; set; }

    [Required, MaxLength(160)]
    public string Name { get; set; } = "";

    [Required, MaxLength(180)]
    public string Slug { get; set; } = "";

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
}
