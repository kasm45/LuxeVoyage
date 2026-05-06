using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models;

public class Attraction
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(200)]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "City is required.")]
    [StringLength(120)]
    public string City { get; set; } = "";

    [Required(ErrorMessage = "Category is required.")]
    [StringLength(80)]
    public string Category { get; set; } = "";

    [StringLength(500)]
    public string? ImageUrl { get; set; }
}
