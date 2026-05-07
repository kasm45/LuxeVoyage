using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models;

public class Notification
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = "";

    public ApplicationUser? User { get; set; }

    [Required, MaxLength(140)]
    public string Title { get; set; } = "";

    [Required, MaxLength(1200)]
    public string Message { get; set; } = "";

    [Required, MaxLength(80)]
    public string Type { get; set; } = "";

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int? ReservationId { get; set; }
    public Booking? Reservation { get; set; }
}
