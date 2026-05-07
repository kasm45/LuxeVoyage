using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models;

public class Booking
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = "";

    public ApplicationUser? User { get; set; }

    public int? TourId { get; set; }
    public Tour? Tour { get; set; }

    public int? StayId { get; set; }
    public Stay? Stay { get; set; }

    public int? ExperienceId { get; set; }
    public Experience? Experience { get; set; }

    public int? DestinationId { get; set; }
    public Destination? Destination { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    [Range(1, 20)]
    public int Guests { get; set; } = 2;

    [MaxLength(500)]
    public string? Notes { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    public DateTime? DecisionedAtUtc { get; set; }
    public string? DecisionedByUserId { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
