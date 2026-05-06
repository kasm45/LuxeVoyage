using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models;

public class Favorite
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = "";

    public FavoriteTargetKind TargetKind { get; set; }

    public int TargetId { get; set; }

    /// <summary>UTC time when the favorite row was created (set on insert).</summary>
    public DateTime CreatedAt { get; set; }
}
