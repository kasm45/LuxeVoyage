using System.Security.Claims;
using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Helpers;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Authorize]
[Route("payments")]
public class PaymentsController : Controller
{
    private const string DemoSuccessDigits = "4242424242424242";
    private const string DemoFailDigits = "4000000000000002";

    private readonly ApplicationDbContext _db;

    public PaymentsController(ApplicationDbContext db) => _db = db;

    [HttpGet("checkout/{bookingId:int}")]
    public async Task<IActionResult> Checkout(int bookingId)
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        var booking = await LoadBookingForPaymentAsync(bookingId);
        if (booking == null || booking.UserId != uid)
            return NotFound();

        if (!CanAttemptPayment(booking))
        {
            SetCannotPayMessage(booking);
            return RedirectToAction("Confirmation", "Bookings", new { id = bookingId });
        }

        if (await HasSuccessfulPaymentAsync(bookingId))
        {
            TempData["Message"] = "Payment has already been recorded for this trip.";
            return RedirectToAction(nameof(Success), new { bookingId });
        }

        var quote = BookingPaymentCalculator.TryGetPayableAmount(booking);
        var vm = MergeCheckoutVm(booking, quote, null);

        ViewBag.NavSection = "Book";
        ViewData["Title"] = "Checkout | LuxeVoyage";
        return View(vm);
    }

    [HttpPost("checkout/{bookingId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(int bookingId, PaymentCheckoutViewModel posted)
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        var booking = await LoadBookingForPaymentAsync(bookingId);
        if (booking == null || booking.UserId != uid)
            return NotFound();

        if (!CanAttemptPayment(booking))
        {
            SetCannotPayMessage(booking);
            return RedirectToAction("Confirmation", "Bookings", new { id = bookingId });
        }

        if (await HasSuccessfulPaymentAsync(bookingId))
        {
            TempData["Message"] = "Payment has already been recorded for this trip.";
            return RedirectToAction(nameof(Success), new { bookingId });
        }

        var quote = BookingPaymentCalculator.TryGetPayableAmount(booking);
        if (!quote.CanPay || quote.Amount <= 0)
        {
            TempData["Error"] = "Your concierge will confirm the final quote before payment.";
            return RedirectToAction(nameof(Checkout), new { bookingId });
        }

        NormalizeCheckoutPost(posted);

        if (!TryValidatePaymentInputs(posted, out var cardDigits))
        {
            ViewBag.NavSection = "Book";
            ViewData["Title"] = "Checkout | LuxeVoyage";
            return View(MergeCheckoutVm(booking, quote, posted));
        }

        var brand = InferCardBrand(cardDigits);
        var last4 = cardDigits.Length >= 4 ? cardDigits.Substring(cardDigits.Length - 4) : cardDigits;

        if (cardDigits == DemoFailDigits)
        {
            _db.Payments.Add(new Payment
            {
                BookingId = bookingId,
                UserId = uid,
                Amount = quote.Amount,
                Currency = "USD",
                Status = PaymentStatus.Failed,
                PaymentMethodBrand = brand,
                Last4 = last4,
                BillingName = posted.BillingName.Trim(),
                BillingEmail = posted.BillingEmail.Trim(),
                BillingPhone = posted.BillingPhone?.Trim(),
                BillingCountry = posted.BillingCountry?.Trim(),
                BillingCity = posted.BillingCity?.Trim(),
                BillingAddressLine = posted.BillingAddressLine?.Trim(),
                CreatedAtUtc = DateTime.UtcNow,
                FailureReason = "Demo decline — use card 4242 4242 4242 4242 for success.",
                TransactionReference = TruncatePaymentRef($"LV-FAIL-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}")
            });
            await _db.SaveChangesAsync();

            ModelState.AddModelError(string.Empty,
                "Payment could not be completed. Please try the demo success card.");
            ViewBag.NavSection = "Book";
            ViewData["Title"] = "Checkout | LuxeVoyage";
            return View(MergeCheckoutVm(booking, quote, posted));
        }

        if (cardDigits != DemoSuccessDigits)
        {
            ModelState.AddModelError(nameof(posted.CardNumber),
                "Use the demo success card 4242 4242 4242 4242 for this portfolio checkout.");
            ViewBag.NavSection = "Book";
            ViewData["Title"] = "Checkout | LuxeVoyage";
            return View(MergeCheckoutVm(booking, quote, posted));
        }

        var txn = TruncatePaymentRef($"LV-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid():N}");
        _db.Payments.Add(new Payment
        {
            BookingId = bookingId,
            UserId = uid,
            Amount = quote.Amount,
            Currency = "USD",
            Status = PaymentStatus.Paid,
            PaymentMethodBrand = brand,
            Last4 = last4,
            BillingName = posted.BillingName.Trim(),
            BillingEmail = posted.BillingEmail.Trim(),
            BillingPhone = posted.BillingPhone?.Trim(),
            BillingCountry = posted.BillingCountry?.Trim(),
            BillingCity = posted.BillingCity?.Trim(),
            BillingAddressLine = posted.BillingAddressLine?.Trim(),
            CreatedAtUtc = DateTime.UtcNow,
            PaidAtUtc = DateTime.UtcNow,
            TransactionReference = txn
        });
        await _db.SaveChangesAsync();

        TempData["Message"] =
            $"Demo payment received — ${quote.Amount:0} USD credited to LuxeVoyage (simulation). Reference {txn}.";
        return RedirectToAction(nameof(Success), new { bookingId });
    }

    [HttpGet("success/{bookingId:int}")]
    public async Task<IActionResult> Success(int bookingId)
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        var booking = await LoadBookingForPaymentAsync(bookingId);
        if (booking == null || booking.UserId != uid)
            return NotFound();

        var payment = await _db.Payments.AsNoTracking()
            .Where(p => p.BookingId == bookingId && p.Status == PaymentStatus.Paid)
            .OrderByDescending(p => p.PaidAtUtc)
            .FirstOrDefaultAsync();

        ViewBag.NavSection = "Book";
        ViewData["Title"] = "Payment received | LuxeVoyage";
        return View(new PaymentSuccessViewModel { Booking = booking, Payment = payment });
    }

    private async Task<Booking?> LoadBookingForPaymentAsync(int bookingId)
    {
        return await _db.Bookings
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .FirstOrDefaultAsync(b => b.Id == bookingId);
    }

    private static bool CanAttemptPayment(Booking b) =>
        b.Status == BookingStatus.Accepted;

    private void SetCannotPayMessage(Booking b)
    {
        TempData["Error"] = b.Status switch
        {
            BookingStatus.Pending =>
                "Payment will be available once our concierge team approves your request.",
            BookingStatus.Rejected =>
                "This request was not available for the selected dates.",
            BookingStatus.Cancelled =>
                "This reservation was cancelled.",
            _ => "This reservation cannot be paid."
        };
    }

    private Task<bool> HasSuccessfulPaymentAsync(int bookingId)
    {
        return _db.Payments.AnyAsync(p =>
            p.BookingId == bookingId && p.Status == PaymentStatus.Paid);
    }

    private static PaymentCheckoutViewModel MergeCheckoutVm(Booking booking,
        (bool CanPay, decimal Amount, string Summary) quote,
        PaymentCheckoutViewModel? posted)
    {
        var title = booking.Tour?.Title ?? booking.Stay?.Name ?? booking.Experience?.Title ?? booking.Destination?.Title ??
                    "Trip";
        var kind = booking.Tour != null ? "Tour"
            : booking.Stay != null ? "Stay"
            : booking.Experience != null ? "Experience"
            : booking.Destination != null ? "Destination"
            : "Trip";

        var vm = new PaymentCheckoutViewModel
        {
            BookingId = booking.Id,
            TripTitle = title,
            KindLabel = kind,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            Guests = booking.Guests,
            CanPay = quote.CanPay && quote.Amount > 0,
            AmountDue = quote.Amount,
            Currency = "USD",
            AmountSummary = quote.Summary
        };

        if (posted != null)
        {
            vm.BillingName = posted.BillingName;
            vm.BillingEmail = posted.BillingEmail;
            vm.BillingPhone = posted.BillingPhone;
            vm.BillingCountry = posted.BillingCountry;
            vm.BillingCity = posted.BillingCity;
            vm.BillingAddressLine = posted.BillingAddressLine;
            vm.CardholderName = posted.CardholderName;
            vm.CardNumber = posted.CardNumber;
            vm.ExpiryMonth = posted.ExpiryMonth;
            vm.ExpiryYear = posted.ExpiryYear;
            vm.Cvv = posted.Cvv;
        }

        return vm;
    }

    private static void NormalizeCheckoutPost(PaymentCheckoutViewModel model)
    {
        model.BillingName = model.BillingName?.Trim() ?? "";
        model.BillingEmail = model.BillingEmail?.Trim() ?? "";
        model.CardholderName = model.CardholderName?.Trim() ?? "";
        model.CardNumber = model.CardNumber?.Trim() ?? "";

        var monthDigits = new string((model.ExpiryMonth ?? "").Where(char.IsDigit).ToArray());
        model.ExpiryMonth = monthDigits.Length <= 2 ? monthDigits : monthDigits[..2];

        var yearDigits = new string((model.ExpiryYear ?? "").Where(char.IsDigit).ToArray());
        model.ExpiryYear = yearDigits.Length <= 4 ? yearDigits : yearDigits[..4];

        var cvvDigits = new string((model.Cvv ?? "").Where(char.IsDigit).ToArray());
        model.Cvv = cvvDigits.Length <= 4 ? cvvDigits : cvvDigits[..4];
    }

    private bool TryValidatePaymentInputs(PaymentCheckoutViewModel model, out string digitsOnly)
    {
        digitsOnly = new string((model.CardNumber ?? "").Where(char.IsDigit).ToArray());

        if (string.IsNullOrWhiteSpace(model.BillingName))
            ModelState.AddModelError(nameof(model.BillingName), "Full name is required.");
        if (string.IsNullOrWhiteSpace(model.BillingEmail))
            ModelState.AddModelError(nameof(model.BillingEmail), "Email is required.");
        if (string.IsNullOrWhiteSpace(model.CardholderName))
            ModelState.AddModelError(nameof(model.CardholderName), "Cardholder name is required.");
        if (digitsOnly.Length is < 13 or > 19)
            ModelState.AddModelError(nameof(model.CardNumber), "Enter a valid card number for the demo.");

        ValidateExpiry(model);
        ValidateCvvDigits(model);

        return ModelState.IsValid;
    }

    /// <summary>Expiry month 1–12; year two-digit → 20YY; reject past expiry (calendar month).</summary>
    private void ValidateExpiry(PaymentCheckoutViewModel model)
    {
        var monthRaw = model.ExpiryMonth?.Trim() ?? "";
        var yearRaw = model.ExpiryYear?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(monthRaw))
        {
            ModelState.AddModelError(nameof(model.ExpiryMonth), "Expiry month is required.");
            return;
        }

        if (string.IsNullOrWhiteSpace(yearRaw))
        {
            ModelState.AddModelError(nameof(model.ExpiryYear), "Expiry year is required.");
            return;
        }

        var monthDigits = new string(monthRaw.Where(char.IsDigit).ToArray());
        if (!int.TryParse(monthDigits, System.Globalization.NumberStyles.Integer, null, out var month) ||
            month is < 1 or > 12)
        {
            ModelState.AddModelError(nameof(model.ExpiryMonth), "Enter a valid expiry month between 01 and 12.");
            return;
        }

        var yearDigits = new string(yearRaw.Where(char.IsDigit).ToArray());
        if (yearDigits.Length is < 1 or > 4 || yearDigits.Length == 3)
        {
            ModelState.AddModelError(nameof(model.ExpiryYear), "Enter a valid expiry year (YY or YYYY).");
            return;
        }

        int fullYear;
        if (yearDigits.Length <= 2)
        {
            if (!int.TryParse(yearDigits, System.Globalization.NumberStyles.Integer, null, out var yy))
            {
                ModelState.AddModelError(nameof(model.ExpiryYear), "Enter a valid expiry year.");
                return;
            }

            fullYear = 2000 + yy;
        }
        else
        {
            if (!int.TryParse(yearDigits, System.Globalization.NumberStyles.Integer, null, out fullYear) ||
                fullYear < 2000 || fullYear > 2099)
            {
                ModelState.AddModelError(nameof(model.ExpiryYear), "Enter a valid expiry year.");
                return;
            }
        }

        var now = DateTime.UtcNow;
        if (fullYear < now.Year || (fullYear == now.Year && month < now.Month))
            ModelState.AddModelError(nameof(model.ExpiryYear), "Enter a valid future expiry date.");
    }

    private void ValidateCvvDigits(PaymentCheckoutViewModel model)
    {
        var cvvDigits = new string((model.Cvv ?? "").Where(char.IsDigit).ToArray());
        if (cvvDigits.Length is not (3 or 4))
            ModelState.AddModelError(nameof(model.Cvv), "Enter a valid 3 or 4 digit security code.");
    }

    private static string InferCardBrand(string digits)
    {
        if (digits.Length == 0)
            return "Card";
        return digits[0] switch
        {
            '4' => "Visa",
            '5' => "Mastercard",
            '3' => "Amex",
            '6' => "Discover",
            _ => "Card"
        };
    }

    private static string TruncatePaymentRef(string raw)
    {
        const int max = 48;
        return raw.Length <= max ? raw : raw.Substring(0, max);
    }
}
