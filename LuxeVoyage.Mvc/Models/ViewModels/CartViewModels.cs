namespace LuxeVoyage.Mvc.Models.ViewModels;

public class CartIndexViewModel
{
    public List<CartRowViewModel> Rows { get; set; } = new();

    /// <summary>Awaiting staff review — payment not available.</summary>
    public List<PendingBookingRowViewModel> WaitingForReview { get; set; } = new();

    /// <summary>Accepted by concierge — demo checkout available.</summary>
    public List<PendingBookingRowViewModel> ReadyForPayment { get; set; } = new();

    /// <summary>Paid — short reinforcement with link to My Trips.</summary>
    public List<PendingBookingRowViewModel> RecentlyPaid { get; set; } = new();
}

public class PendingBookingRowViewModel
{
    public int BookingId { get; set; }

    /// <summary>Destination, Experience, Tour, or Stay.</summary>
    public string KindBadge { get; set; } = "";

    public string Title { get; set; } = "";
    public string? ImageUrl { get; set; }

    /// <summary>Single line for UI — includes &quot;Estimated · …&quot; or &quot;Custom quote&quot;.</summary>
    public string PriceDisplayLine { get; set; } = "Custom quote";

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Guests { get; set; }
    public string DetailUrl { get; set; } = "#";
    public string ConfirmationUrl { get; set; } = "#";

    /// <summary>Demo checkout — URL when payment can be taken.</summary>
    public string CheckoutUrl { get; set; } = "#";

    /// <summary>Payment success / receipt after demo checkout.</summary>
    public string PaymentReceiptUrl { get; set; } = "#";

    public bool HasPaid { get; set; }
    public decimal? PaidAmountUsd { get; set; }
    public bool ShowPayNow { get; set; }
    public bool ShowAwaitingQuote { get; set; }

    /// <summary>Primary status line for the reservation card.</summary>
    public string StatusHeadline { get; set; } = "";

    /// <summary>Secondary payment / next-step hint.</summary>
    public string PaymentHint { get; set; } = "";

    public string MyTripsUrl { get; set; } = "#";
}

public class CartRowViewModel
{
    public int CartItemId { get; set; }
    public string ItemType { get; set; } = "";
    public int ItemId { get; set; }
    public string Title { get; set; } = "";
    public string? ImageUrl { get; set; }
    public string TypeLabel { get; set; } = "";
    public string LocationLine { get; set; } = "";

    /// <summary>Catalog-style hint or &quot;Custom quote&quot;.</summary>
    public string PriceDisplayLine { get; set; } = "Custom quote";

    public string DetailUrl { get; set; } = "#";
    public string BookingUrl { get; set; } = "#";
}

public class AddToCartButtonModel
{
    public string ItemType { get; set; } = "";
    public int ItemId { get; set; }
    public string ReturnUrl { get; set; } = "";
}
