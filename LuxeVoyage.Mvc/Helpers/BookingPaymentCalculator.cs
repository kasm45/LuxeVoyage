using LuxeVoyage.Mvc.Models;

namespace LuxeVoyage.Mvc.Helpers;

/// <summary>Computes demo checkout totals from catalog hints — no payment provider.</summary>
public static class BookingPaymentCalculator
{
    /// <summary>Numeric estimate from catalog pricing hints (any booking status).</summary>
    public static (bool HasEstimate, decimal Amount, string Summary) TryGetEstimatedQuote(Booking b)
    {
        if (b.TourId != null && b.Tour != null)
        {
            var amt = Math.Round(b.Tour.Price * b.Guests, 2);
            return amt > 0 ? (true, amt, $"Tour × {b.Guests} guests") : (false, 0, "");
        }

        if (b.StayId != null && b.Stay != null)
        {
            var nights = Math.Max(1, (b.EndDate.Date - b.StartDate.Date).Days);
            var amt = Math.Round(nights * b.Stay.PricePerNight, 2);
            return amt > 0 ? (true, amt, $"{nights} night(s) × nightly rate") : (false, 0, "");
        }

        if (b.ExperienceId != null && b.Experience != null)
        {
            var e = b.Experience;
            var hint = string.IsNullOrWhiteSpace(e.CardPriceHint) ? e.PriceHint : e.CardPriceHint;
            var unit = MoneyHintParser.TryParseUsd(hint);
            if (unit is null || unit <= 0)
                return (false, 0, "");
            var amt = Math.Round(unit.Value * b.Guests, 2);
            return (true, amt, $"Estimate × {b.Guests} guests");
        }

        if (b.DestinationId != null && b.Destination != null)
        {
            var d = b.Destination;
            var hint = string.IsNullOrWhiteSpace(d.CardPriceHint) ? d.PriceHint : d.CardPriceHint;
            var unit = MoneyHintParser.TryParseUsd(hint);
            if (unit is null || unit <= 0)
                return (false, 0, "");
            var amt = Math.Round(unit.Value * b.Guests, 2);
            return (true, amt, $"Estimate × {b.Guests} travelers");
        }

        return (false, 0, "");
    }

    /// <summary>Payable only after concierge accepts the booking (<see cref="BookingStatus.Accepted"/>).</summary>
    public static (bool CanPay, decimal Amount, string Summary) TryGetPayableAmount(Booking b)
    {
        if (b.Status != BookingStatus.Accepted)
            return (false, 0, "");

        var est = TryGetEstimatedQuote(b);
        return est.HasEstimate && est.Amount > 0 ? (true, est.Amount, est.Summary) : (false, 0, "");
    }
}
