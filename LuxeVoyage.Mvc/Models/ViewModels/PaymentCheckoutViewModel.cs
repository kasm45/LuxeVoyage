using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models.ViewModels;

public class PaymentCheckoutViewModel
{
    public int BookingId { get; set; }
    public string TripTitle { get; set; } = "";
    public string KindLabel { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Guests { get; set; }

    public bool CanPay { get; set; }
    public decimal AmountDue { get; set; }
    public string Currency { get; set; } = "USD";
    public string AmountSummary { get; set; } = "";

    [Required, MaxLength(160)]
    public string BillingName { get; set; } = "";

    [Required, EmailAddress, MaxLength(256)]
    public string BillingEmail { get; set; } = "";

    [Phone, MaxLength(40)]
    public string? BillingPhone { get; set; }

    [MaxLength(80)]
    public string? BillingCountry { get; set; }

    [MaxLength(120)]
    public string? BillingCity { get; set; }

    [MaxLength(300)]
    public string? BillingAddressLine { get; set; }

    [Required, MaxLength(160)]
    public string CardholderName { get; set; } = "";

    /// <summary>Demo only — not stored.</summary>
    [Required, MaxLength(24)]
    public string CardNumber { get; set; } = "";

    [Required, MaxLength(2)]
    public string ExpiryMonth { get; set; } = "";

    [Required, MaxLength(4)]
    public string ExpiryYear { get; set; } = "";

    /// <summary>Demo only — never persisted.</summary>
    [Required, MaxLength(8)]
    public string Cvv { get; set; } = "";
}
