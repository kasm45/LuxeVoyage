using LuxeVoyage.Mvc.Models;

namespace LuxeVoyage.Mvc.Models.ViewModels;

public sealed class AccountReservationsPageVm
{
    public IReadOnlyList<ReservationTripRowVm> AwaitingReview { get; init; } = Array.Empty<ReservationTripRowVm>();
    public IReadOnlyList<ReservationTripRowVm> PaymentDue { get; init; } = Array.Empty<ReservationTripRowVm>();
    public IReadOnlyList<ReservationTripRowVm> UpcomingPaidTrips { get; init; } = Array.Empty<ReservationTripRowVm>();
    public IReadOnlyList<ReservationTripRowVm> PastTrips { get; init; } = Array.Empty<ReservationTripRowVm>();
}

public sealed class ReservationTripRowVm
{
    public Booking Booking { get; set; } = null!;
    public Payment? PaidPayment { get; set; }
    public bool HasFailedPaymentAttempt { get; set; }
    public string CheckoutUrl { get; set; } = "#";
    public string ConfirmationUrl { get; set; } = "#";

    public bool CanPayDemo { get; set; }
    public decimal? AmountDueUsd { get; set; }
}
