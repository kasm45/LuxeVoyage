using LuxeVoyage.Mvc.Models;

namespace LuxeVoyage.Mvc.Models.ViewModels;

public sealed class AdminUserRowViewModel
{
    public string Id { get; init; } = "";
    public string Email { get; init; } = "";
    public string DisplayName { get; init; } = "";
    public string RolesSummary { get; init; } = "";
    public bool EmailConfirmed { get; init; }
    public int ReservationsCount { get; init; }
    public int FavoritesCount { get; init; }
    public bool IsAdmin { get; init; }
    public bool IsPersonnel { get; init; }
    public bool HasRelatedRecords { get; init; }
    /// <summary>True when Identity lockout prevents sign-in (legacy disable-only, not anonymized).</summary>
    public bool IsDisabled { get; init; }
    /// <summary>Login anonymized (archived email); history retained under same UserId.</summary>
    public bool IsRemoved { get; init; }
    /// <summary>Locked out but not yet archived — can use legacy Reactivate.</summary>
    public bool CanReactivate { get; init; }
}

public sealed class AdminAnalyticsViewModel
{
    public int TotalUsers { get; init; }
    public int TotalBookings { get; init; }
    public int PendingBookings { get; init; }
    public int ConfirmedBookings { get; init; }
    public int TotalDestinations { get; init; }
    public int TotalExperiences { get; init; }
    public int TotalTours { get; init; }
    public int TotalStays { get; init; }
    public int TotalFavorites { get; init; }
    public string MostReservedItem { get; init; } = "—";
    public string MostFavoritedItem { get; init; } = "—";
    public string LatestUserEmail { get; init; } = "—";

    public IReadOnlyList<AdminAnalyticsBookingRowViewModel> RecentBookings { get; init; } =
        Array.Empty<AdminAnalyticsBookingRowViewModel>();
}

public sealed class AdminAnalyticsBookingRowViewModel
{
    public int Id { get; init; }
    public string GuestEmail { get; init; } = "";
    public string ItemLabel { get; init; } = "";
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public BookingStatus Status { get; init; }
    public DateTime CreatedAtUtc { get; init; }
}

public sealed class AdminSettingsViewModel
{
    public string SiteName { get; init; } = "LuxeVoyage";
    public string EnvironmentName { get; init; } = "";
    public string DatabaseProvider { get; init; } = "";
    public int CurrentYear { get; init; }
    public string DemoNote { get; init; } =
        "This deployment uses seeded demo data and local SQLite storage. Do not use for production without configuration review.";
}

public sealed class PaymentSuccessViewModel
{
    public Booking Booking { get; set; } = null!;
    public Payment? Payment { get; set; }
}

public sealed class AdminPendingReservationRowVm
{
    public Booking Booking { get; set; } = null!;
    public Payment? PaidPayment { get; set; }
    public bool HasFailedPaymentAttempt { get; set; }
    public bool QuoteAvailable { get; set; }
    public decimal? QuoteAmount { get; set; }
}

public sealed class AdminDashboardMainViewModel
{
    public decimal TotalPaidRevenueUsd { get; init; }
    public int PaidBookingsCount { get; init; }
    public int PendingUnpaidRequestsCount { get; init; }
    public IReadOnlyList<AdminRecentPaymentRowVm> RecentPaidPayments { get; init; } =
        Array.Empty<AdminRecentPaymentRowVm>();

    public string DemoRevenueNote { get; init; } =
        "Demo revenue — amounts simulate funds credited to the LuxeVoyage portfolio account (no bank transfer).";
}

public sealed class AdminRecentPaymentRowVm
{
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "USD";
    public DateTime? PaidAtUtc { get; init; }
    public string? TransactionReference { get; init; }
    public string GuestEmail { get; init; } = "";
    public string ItemLabel { get; init; } = "";
}
