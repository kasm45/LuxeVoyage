using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models;

public class CartItem
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = "";

    /// <summary>Experience, Tour, Stay, or Destination (canonical casing).</summary>
    [Required, MaxLength(20)]
    public string ItemType { get; set; } = "";

    public int ItemId { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;
}
