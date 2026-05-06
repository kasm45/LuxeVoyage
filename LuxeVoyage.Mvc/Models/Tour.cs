using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models;

public class Tour
{
    public int Id { get; set; }

    [Required, MaxLength(160)]
    public string Title { get; set; } = "";

    [Required, MaxLength(180)]
    public string Slug { get; set; } = "";

    public decimal Price { get; set; }
    public int DurationDays { get; set; }

    [MaxLength(80)]
    public string CategoryLabel { get; set; } = "";

    [MaxLength(400)]
    public string? ImageUrl { get; set; }

    [MaxLength(600)]
    public string? Summary { get; set; }

    public double Rating { get; set; }
    public int ReviewCount { get; set; }
}
