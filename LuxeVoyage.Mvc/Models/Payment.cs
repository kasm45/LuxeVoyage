using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models;

/// <summary>Demo payment record — no full PAN, no CVV persisted.</summary>
public class Payment
{
    public int Id { get; set; }

    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;

    [Required]
    public string UserId { get; set; } = "";

    public ApplicationUser? User { get; set; }

    [Range(0.01, 999999)]
    public decimal Amount { get; set; }

    [Required, MaxLength(8)]
    public string Currency { get; set; } = "USD";

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    [MaxLength(40)]
    public string? PaymentMethodBrand { get; set; }

    [MaxLength(4)]
    public string? Last4 { get; set; }

    [Required, MaxLength(160)]
    public string BillingName { get; set; } = "";

    [Required, MaxLength(256)]
    public string BillingEmail { get; set; } = "";

    [MaxLength(40)]
    public string? BillingPhone { get; set; }

    [MaxLength(80)]
    public string? BillingCountry { get; set; }

    [MaxLength(120)]
    public string? BillingCity { get; set; }

    [MaxLength(300)]
    public string? BillingAddressLine { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? PaidAtUtc { get; set; }

    [MaxLength(48)]
    public string? TransactionReference { get; set; }

    [MaxLength(500)]
    public string? FailureReason { get; set; }
}
