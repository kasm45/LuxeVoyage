using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models.ViewModels;

public class BookingCreateViewModel
{
    public int? TourId { get; set; }
    public int? StayId { get; set; }
    public int? ExperienceId { get; set; }
    public int? DestinationId { get; set; }

    public string? TourTitle { get; set; }
    public string? StayName { get; set; }
    public string? ExperienceTitle { get; set; }
    public string? DestinationTitle { get; set; }
    public decimal? PriceHint { get; set; }
    public string? CapacityLabel { get; set; }
    public string? BookingKind { get; set; }
    public int? MinGuests { get; set; }
    public int? MaxGuests { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; } = DateTime.Today.AddDays(14);

    [Required]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(21);

    [Range(1, 20)]
    public int Guests { get; set; } = 2;

    [MaxLength(500)]
    public string? Notes { get; set; }
}
