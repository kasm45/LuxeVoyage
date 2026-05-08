using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Helpers;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Route("bookings")]
public class BookingsController : Controller
{
    private readonly ApplicationDbContext _db;

    public BookingsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create(int? tourId, int? stayId, int? experienceId, int? destinationId,
        DateTime? startDate, DateTime? endDate, int? guests)
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            var returnUrl = Url.Action(nameof(Create), "Bookings",
                new { tourId, stayId, experienceId, destinationId, startDate, endDate, guests })!;
            return RedirectToAction(nameof(AccountController.Login), "Account", new { returnUrl });
        }

        var model = new BookingCreateViewModel();

        if (tourId is > 0)
        {
            var tour = await _db.Tours.AsNoTracking().FirstOrDefaultAsync(t => t.Id == tourId && t.IsActive);
            if (tour != null)
            {
                model.TourId = tour.Id;
                model.TourTitle = tour.Title;
                model.PriceHint = tour.Price;
                model.BookingKind = "tour";
                model.CapacityLabel = tour.GroupSizeText;
                ApplyCapacityRange(model, tour.GroupSizeText);
            }
        }

        if (stayId is > 0)
        {
            var stay = await _db.Stays.AsNoTracking().FirstOrDefaultAsync(s => s.Id == stayId && s.IsActive);
            if (stay != null)
            {
                model.StayId = stay.Id;
                model.StayName = stay.Name;
                model.PriceHint = stay.PricePerNight;
                model.BookingKind = "stay";
                model.CapacityLabel = stay.GuestCapacity;
                ApplyCapacityRange(model, stay.GuestCapacity);
            }
        }

        if (experienceId is > 0)
        {
            var ex = await _db.Experiences.AsNoTracking().FirstOrDefaultAsync(e => e.Id == experienceId && e.IsActive);
            if (ex != null)
            {
                model.ExperienceId = ex.Id;
                model.ExperienceTitle = ex.Title;
                model.BookingKind = "experience";
                model.CapacityLabel = ex.GroupSizeText;
                ApplyCapacityRange(model, ex.GroupSizeText);
            }
        }

        if (destinationId is > 0)
        {
            var d = await _db.Destinations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == destinationId && x.IsActive);
            if (d != null)
            {
                model.DestinationId = d.Id;
                model.DestinationTitle = d.Title;
                model.BookingKind = "destination";
                model.PriceHint = MoneyHintParser.TryParseUsd(d.PriceHint) ?? MoneyHintParser.TryParseUsd(d.CardPriceHint);
            }
        }

        ApplyOptionalBookingDefaults(model, startDate, endDate, guests);

        if (model.TourId == null && model.StayId == null && model.ExperienceId == null &&
            model.DestinationId == null)
        {
            TempData["Error"] = "Select an experience, tour, stay, or destination to book.";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        ViewBag.NavSection = "Book";
        ViewData["Title"] = "Create reservation | LuxeVoyage";
        return View(model);
    }

    [HttpPost("create")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookingCreateViewModel model)
    {
        var uid = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        var countTargets = (model.TourId != null ? 1 : 0) + (model.StayId != null ? 1 : 0) +
                           (model.ExperienceId != null ? 1 : 0) + (model.DestinationId != null ? 1 : 0);
        if (countTargets != 1)
            ModelState.AddModelError(string.Empty, "Choose exactly one item to book.");

        await PopulateBookingContextAsync(model);
        var isStay = model.StayId.HasValue;
        if (isStay && model.EndDate <= model.StartDate)
            ModelState.AddModelError(nameof(model.EndDate), "For stays, end date must be after the start date.");
        else if (!isStay && model.EndDate < model.StartDate)
            ModelState.AddModelError(nameof(model.EndDate), "End date must be on or after the start date.");

        if (model.Guests <= 0)
            ModelState.AddModelError(nameof(model.Guests), "Guest count must be at least 1.");

        if (model.MinGuests is int minGuests && model.MaxGuests is int maxGuests &&
            (model.Guests < minGuests || model.Guests > maxGuests))
        {
            ModelState.AddModelError(
                nameof(model.Guests),
                BuildCapacityErrorMessage(model.BookingKind, minGuests, maxGuests));
        }

        if (!ModelState.IsValid)
        {
            ViewBag.NavSection = "Book";
            ViewData["Title"] = "Create reservation | LuxeVoyage";
            return View(model);
        }

        var matchingCart = await FindMatchingCartItemAsync(uid, model);

        var duplicate = await FindDuplicatePendingBookingAsync(uid, model);
        if (duplicate != null)
        {
            if (matchingCart != null)
            {
                _db.CartItems.Remove(matchingCart);
                await _db.SaveChangesAsync();
                TempData["Message"] =
                    "This trip is already waiting for concierge review. We moved it from Saved for later to Requested trips.";
            }
            else
            {
                TempData["Message"] =
                    "This trip is already waiting for concierge review. You can follow it under Requested trips.";
            }

            return RedirectToAction("Index", "Cart");
        }

        var booking = new Booking
        {
            UserId = uid,
            TourId = model.TourId,
            StayId = model.StayId,
            ExperienceId = model.ExperienceId,
            DestinationId = model.DestinationId,
            StartDate = model.StartDate.Date,
            EndDate = model.EndDate.Date,
            Guests = model.Guests,
            Notes = model.Notes,
            Status = BookingStatus.Pending,
            CreatedAtUtc = DateTime.UtcNow
        };

        var traveler = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == uid);
        if (traveler != null)
        {
            booking.CustomerNameSnapshot = string.IsNullOrWhiteSpace(traveler.DisplayName)
                ? null
                : traveler.DisplayName.Trim();
            booking.CustomerEmailSnapshot = string.IsNullOrWhiteSpace(traveler.Email)
                ? null
                : traveler.Email.Trim();
        }

        _db.Bookings.Add(booking);
        if (matchingCart != null)
            _db.CartItems.Remove(matchingCart);

        await _db.SaveChangesAsync();
        TempData["Message"] =
            "Your request has been sent. We moved it to Requested trips while our concierge team reviews availability.";
        return RedirectToAction("Index", "Cart");
    }

    [Authorize]
    [HttpGet("confirmation/{id:int}")]
    public async Task<IActionResult> Confirmation(int id)
    {
        var uid = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        var booking = await _db.Bookings.AsNoTracking()
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == uid);

        if (booking == null)
        {
            TempData["Error"] = "Reservation could not be found.";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        var paid = await _db.Payments.AsNoTracking()
            .Where(p => p.BookingId == id && p.Status == PaymentStatus.Paid)
            .OrderByDescending(p => p.PaidAtUtc)
            .FirstOrDefaultAsync();

        var quote = BookingPaymentCalculator.TryGetPayableAmount(booking);
        ViewBag.PaidPayment = paid;
        ViewBag.QuoteCanPay = quote.CanPay && quote.Amount > 0;
        ViewBag.QuoteAmount = quote.Amount;
        ViewBag.QuoteSummary = quote.Summary;
        ViewBag.CheckoutUrl = Url.Action("Checkout", "Payments", new { bookingId = id });
        ViewBag.PaymentSuccessUrl = Url.Action("Success", "Payments", new { bookingId = id });

        ViewBag.NavSection = "Book";
        ViewData["Title"] = "Trip request received | LuxeVoyage";
        return View(booking);
    }

    private async Task PopulateBookingContextAsync(BookingCreateViewModel model)
    {
        model.BookingKind = null;
        model.CapacityLabel = null;
        model.MinGuests = null;
        model.MaxGuests = null;

        if (model.ExperienceId is > 0)
        {
            var ex = await _db.Experiences.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == model.ExperienceId.Value && e.IsActive);
            if (ex == null)
            {
                ModelState.AddModelError(string.Empty, "Selected experience is unavailable.");
                return;
            }

            model.BookingKind = "experience";
            model.ExperienceTitle = ex.Title;
            model.CapacityLabel = ex.GroupSizeText;
            ApplyCapacityRange(model, ex.GroupSizeText);
            return;
        }

        if (model.TourId is > 0)
        {
            var tour = await _db.Tours.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == model.TourId.Value && t.IsActive);
            if (tour == null)
            {
                ModelState.AddModelError(string.Empty, "Selected tour is unavailable.");
                return;
            }

            model.BookingKind = "tour";
            model.TourTitle = tour.Title;
            model.PriceHint = tour.Price;
            model.CapacityLabel = tour.GroupSizeText;
            ApplyCapacityRange(model, tour.GroupSizeText);
            return;
        }

        if (model.StayId is > 0)
        {
            var stay = await _db.Stays.AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == model.StayId.Value && s.IsActive);
            if (stay == null)
            {
                ModelState.AddModelError(string.Empty, "Selected stay is unavailable.");
                return;
            }

            model.BookingKind = "stay";
            model.StayName = stay.Name;
            model.PriceHint = stay.PricePerNight;
            model.CapacityLabel = stay.GuestCapacity;
            ApplyCapacityRange(model, stay.GuestCapacity);
            return;
        }

        if (model.DestinationId is > 0)
        {
            var destination = await _db.Destinations.AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == model.DestinationId.Value && d.IsActive);
            if (destination == null)
            {
                ModelState.AddModelError(string.Empty, "Selected destination is unavailable.");
                return;
            }

            model.BookingKind = "destination";
            model.DestinationTitle = destination.Title;
            model.PriceHint = MoneyHintParser.TryParseUsd(destination.PriceHint) ??
                              MoneyHintParser.TryParseUsd(destination.CardPriceHint);
            return;
        }

        ModelState.AddModelError(string.Empty, "Select an active listing before submitting a reservation.");
    }

    private static void ApplyOptionalBookingDefaults(BookingCreateViewModel model, DateTime? startDate,
        DateTime? endDate, int? guests)
    {
        if (startDate.HasValue)
            model.StartDate = startDate.Value.Date;
        if (endDate.HasValue)
            model.EndDate = endDate.Value.Date;
        if (guests is >= 1 and <= 20)
            model.Guests = guests.Value;
    }

    private async Task<Booking?> FindDuplicatePendingBookingAsync(string userId, BookingCreateViewModel model)
    {
        var sd = model.StartDate.Date;
        var ed = model.EndDate.Date;
        var g = model.Guests;

        var q = _db.Bookings.AsNoTracking()
            .Where(b => b.UserId == userId && b.Status == BookingStatus.Pending);

        if (model.ExperienceId is { } xe)
            return await q.FirstOrDefaultAsync(b =>
                b.ExperienceId == xe && b.StartDate == sd && b.EndDate == ed && b.Guests == g);

        if (model.TourId is { } xt)
            return await q.FirstOrDefaultAsync(b =>
                b.TourId == xt && b.StartDate == sd && b.EndDate == ed && b.Guests == g);

        if (model.StayId is { } xs)
            return await q.FirstOrDefaultAsync(b =>
                b.StayId == xs && b.StartDate == sd && b.EndDate == ed && b.Guests == g);

        if (model.DestinationId is { } xd)
            return await q.FirstOrDefaultAsync(b =>
                b.DestinationId == xd && b.StartDate == sd && b.EndDate == ed && b.Guests == g);

        return null;
    }

    private async Task<CartItem?> FindMatchingCartItemAsync(string userId, BookingCreateViewModel model)
    {
        if (model.ExperienceId is int xe)
            return await _db.CartItems.FirstOrDefaultAsync(c =>
                c.UserId == userId && c.IsActive && c.ItemType == CartItemTypes.Experience && c.ItemId == xe);

        if (model.TourId is int xt)
            return await _db.CartItems.FirstOrDefaultAsync(c =>
                c.UserId == userId && c.IsActive && c.ItemType == CartItemTypes.Tour && c.ItemId == xt);

        if (model.StayId is int xs)
            return await _db.CartItems.FirstOrDefaultAsync(c =>
                c.UserId == userId && c.IsActive && c.ItemType == CartItemTypes.Stay && c.ItemId == xs);

        if (model.DestinationId is int xd)
            return await _db.CartItems.FirstOrDefaultAsync(c =>
                c.UserId == userId && c.IsActive && c.ItemType == CartItemTypes.Destination && c.ItemId == xd);

        return null;
    }

    private static void ApplyCapacityRange(BookingCreateViewModel model, string? rawCapacity)
    {
        if (GuestCapacityParser.TryParseGuestRange(rawCapacity, out var min, out var max))
        {
            model.MinGuests = min;
            model.MaxGuests = max;
        }
    }

    private static string BuildCapacityErrorMessage(string? kind, int min, int max)
    {
        var range = min == max ? $"{min}" : $"{min}-{max}";
        return kind switch
        {
            "experience" => $"This experience supports {range} guests. Please adjust your group size.",
            "tour" => $"This tour supports {range} guests. Please adjust your group size.",
            "stay" => $"This stay supports {range} guests. Please adjust your group size.",
            _ => $"This request supports {range} guests. Please adjust your group size."
        };
    }
}
