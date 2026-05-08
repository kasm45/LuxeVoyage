using System.Security.Claims;
using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Helpers;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Route("cart")]
public class CartController : Controller
{
    private readonly ApplicationDbContext _db;

    public CartController(ApplicationDbContext db) => _db = db;

    [HttpGet("")]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        var items = await _db.CartItems
            .Where(c => c.UserId == uid && c.IsActive)
            .OrderByDescending(c => c.CreatedAtUtc)
            .ToListAsync();

        var rows = new List<CartRowViewModel>();
        foreach (var c in items)
        {
            var row = await TryBuildRowAsync(c);
            if (row == null)
            {
                _db.CartItems.Remove(c);
                continue;
            }

            rows.Add(row);
        }

        await _db.SaveChangesAsync();

        var activeBookings = await _db.Bookings
            .AsNoTracking()
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .Where(b => b.UserId == uid &&
                        (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Accepted))
            .OrderByDescending(b => b.CreatedAtUtc)
            .ToListAsync();

        var bookingIds = activeBookings.Select(x => x.Id).ToList();
        var paidRows = await _db.Payments.AsNoTracking()
            .Where(p => bookingIds.Contains(p.BookingId) && p.Status == PaymentStatus.Paid)
            .ToListAsync();
        var paidByBookingId = paidRows
            .GroupBy(p => p.BookingId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.PaidAtUtc).First());

        var myTripsUrl = Url.Action("Reservations", "Account") ?? "#";

        var waiting = new List<PendingBookingRowViewModel>();
        var ready = new List<PendingBookingRowViewModel>();
        var paidRecent = new List<PendingBookingRowViewModel>();

        foreach (var b in activeBookings)
        {
            paidByBookingId.TryGetValue(b.Id, out var pmt);
            if (b.Status == BookingStatus.Pending)
                waiting.Add(MapPendingBooking(b, null, CartBasketSegment.WaitingReview, myTripsUrl));
            else if (pmt == null)
                ready.Add(MapPendingBooking(b, null, CartBasketSegment.ReadyPayment, myTripsUrl));
            else
                paidRecent.Add(MapPendingBooking(b, pmt, CartBasketSegment.PaidReceipt, myTripsUrl));
        }

        ViewBag.NavSection = "Cart";
        ViewData["Title"] = "Your Travel Basket | LuxeVoyage";
        return View(new CartIndexViewModel
        {
            Rows = rows,
            WaitingForReview = waiting,
            ReadyForPayment = ready,
            RecentlyPaid = paidRecent
        });
    }

    [HttpPost("add")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(string itemType, int itemId, string? returnUrl)
    {
        var ru = returnUrl;
        if (string.IsNullOrEmpty(ru) || !Url.IsLocalUrl(ru))
            ru = Url.Action(nameof(Index), "Cart") ?? "/";

        if (!CartItemTypes.TryNormalize(itemType, out var canonical))
        {
            TempData["Error"] = "That item cannot be added to your basket.";
            return LocalRedirect(ru);
        }

        if (itemId <= 0)
        {
            TempData["Error"] = "That item cannot be added to your basket.";
            return LocalRedirect(ru);
        }

        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            TempData["Message"] = "Please sign in to add trips to your basket.";
            return RedirectToAction(nameof(AccountController.Login), "Account", new { returnUrl = ru });
        }

        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        if (!await CatalogItemExistsAsync(canonical, itemId))
        {
            TempData["Error"] = "This listing is no longer available.";
            return LocalRedirect(ru);
        }

        var exists = await _db.CartItems.AnyAsync(c =>
            c.UserId == uid && c.IsActive && c.ItemType == canonical && c.ItemId == itemId);
        if (exists)
        {
            TempData["Message"] = "This trip is already in your basket.";
            return LocalRedirect(ru);
        }

        _db.CartItems.Add(new CartItem
        {
            UserId = uid,
            ItemType = canonical,
            ItemId = itemId,
            CreatedAtUtc = DateTime.UtcNow,
            IsActive = true
        });
        await _db.SaveChangesAsync();

        TempData["Message"] = "Added to your travel basket.";
        return LocalRedirect(ru);
    }

    [HttpPost("remove/{id:int}")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int id, string? returnUrl)
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        var row = await _db.CartItems.FirstOrDefaultAsync(c => c.Id == id && c.UserId == uid);
        if (row != null)
            _db.CartItems.Remove(row);

        await _db.SaveChangesAsync();

        TempData["Message"] = "Removed from Saved for later.";
        var ru = returnUrl;
        if (!string.IsNullOrEmpty(ru) && Url.IsLocalUrl(ru))
            return LocalRedirect(ru);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("clear")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Clear()
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        var rows = await _db.CartItems.Where(c => c.UserId == uid && c.IsActive).ToListAsync();
        _db.CartItems.RemoveRange(rows);
        await _db.SaveChangesAsync();

        TempData["Message"] = "Saved items cleared.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> CatalogItemExistsAsync(string canonical, int itemId)
    {
        return canonical switch
        {
            CartItemTypes.Experience => await _db.Experiences.AsNoTracking()
                .AnyAsync(e => e.Id == itemId && e.IsActive),
            CartItemTypes.Tour => await _db.Tours.AsNoTracking()
                .AnyAsync(t => t.Id == itemId && t.IsActive),
            CartItemTypes.Stay => await _db.Stays.AsNoTracking()
                .AnyAsync(s => s.Id == itemId && s.IsActive),
            CartItemTypes.Destination => await _db.Destinations.AsNoTracking()
                .AnyAsync(d => d.Id == itemId && d.IsActive),
            _ => false
        };
    }

    private async Task<CartRowViewModel?> TryBuildRowAsync(CartItem c)
    {
        switch (c.ItemType)
        {
            case CartItemTypes.Experience:
            {
                var e = await _db.Experiences.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == c.ItemId && x.IsActive);
                if (e == null)
                    return null;

                var loc = string.Join(" · ",
                    new[] { e.LocationLabel, CatalogQueryHelper.RegionDisplay(e.Region) }
                        .Where(s => !string.IsNullOrWhiteSpace(s)));

                var hint = string.IsNullOrWhiteSpace(e.CardPriceHint) ? e.PriceHint : e.CardPriceHint;

                return new CartRowViewModel
                {
                    CartItemId = c.Id,
                    ItemType = CartItemTypes.Experience,
                    ItemId = e.Id,
                    Title = e.Title,
                    ImageUrl = e.ImageUrl,
                    TypeLabel = "Experience · " + CatalogQueryHelper.CategoryDisplay(e.Category),
                    LocationLine = loc,
                    PriceDisplayLine = CartPriceDisplay.SavedLine(hint),
                    DetailUrl = Url.Action("Detail", "Experiences", new { id = e.Slug }) ?? "#",
                    BookingUrl = Url.Action("Create", "Bookings", new { experienceId = e.Id }) ?? "#"
                };
            }
            case CartItemTypes.Tour:
            {
                var t = await _db.Tours.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == c.ItemId && x.IsActive);
                if (t == null)
                    return null;

                var loc = string.IsNullOrWhiteSpace(t.DestinationLabel)
                    ? CatalogQueryHelper.RegionDisplay(t.Region)
                    : t.DestinationLabel;

                var priceHint = string.IsNullOrWhiteSpace(t.CardPriceHint)
                    ? "$" + t.Price.ToString("0")
                    : t.CardPriceHint;

                return new CartRowViewModel
                {
                    CartItemId = c.Id,
                    ItemType = CartItemTypes.Tour,
                    ItemId = t.Id,
                    Title = t.Title,
                    ImageUrl = string.IsNullOrWhiteSpace(t.CardImageUrl) ? t.ImageUrl : t.CardImageUrl,
                    TypeLabel = "Tour · " + (string.IsNullOrWhiteSpace(t.CategoryLabel) ? "Curated journey" : t.CategoryLabel),
                    LocationLine = loc ?? "",
                    PriceDisplayLine = CartPriceDisplay.SavedLine(priceHint),
                    DetailUrl = Url.Action("Detail", "Tours", new { slug = t.Slug }) ?? "#",
                    BookingUrl = Url.Action("Create", "Bookings", new { tourId = t.Id }) ?? "#"
                };
            }
            case CartItemTypes.Stay:
            {
                var s = await _db.Stays.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == c.ItemId && x.IsActive);
                if (s == null)
                    return null;

                var priceHint = string.IsNullOrWhiteSpace(s.CardPriceHint)
                    ? "$" + s.PricePerNight.ToString("0") + " / night"
                    : s.CardPriceHint;

                return new CartRowViewModel
                {
                    CartItemId = c.Id,
                    ItemType = CartItemTypes.Stay,
                    ItemId = s.Id,
                    Title = string.IsNullOrWhiteSpace(s.CardTitle) ? s.Name : s.CardTitle,
                    ImageUrl = string.IsNullOrWhiteSpace(s.CardImageUrl) ? s.ImageUrl : s.CardImageUrl,
                    TypeLabel = "Stay · " + (string.IsNullOrWhiteSpace(s.StayType) ? "Luxury stay" : s.StayType),
                    LocationLine = s.CityLine + " · " + CatalogQueryHelper.RegionDisplay(s.Region),
                    PriceDisplayLine = CartPriceDisplay.SavedLine(priceHint),
                    DetailUrl = Url.Action("Detail", "Hotels", new { id = s.Slug }) ?? "#",
                    BookingUrl = Url.Action("Create", "Bookings", new { stayId = s.Id }) ?? "#"
                };
            }
            case CartItemTypes.Destination:
            {
                var d = await _db.Destinations.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == c.ItemId && x.IsActive);
                if (d == null)
                    return null;

                var loc = string.Join(" · ",
                    new[] { d.LocationLabel, CatalogQueryHelper.RegionDisplay(d.Region) }
                        .Where(s => !string.IsNullOrWhiteSpace(s)));

                var priceHint = string.IsNullOrWhiteSpace(d.CardPriceHint) ? d.PriceHint : d.CardPriceHint;

                return new CartRowViewModel
                {
                    CartItemId = c.Id,
                    ItemType = CartItemTypes.Destination,
                    ItemId = d.Id,
                    Title = string.IsNullOrWhiteSpace(d.CardTitle) ? d.Title : d.CardTitle,
                    ImageUrl = string.IsNullOrWhiteSpace(d.CardImageUrl) ? d.ImageUrl : d.CardImageUrl,
                    TypeLabel = "Destination · " + CatalogQueryHelper.RegionDisplay(d.Region),
                    LocationLine = loc,
                    PriceDisplayLine = CartPriceDisplay.SavedLine(priceHint),
                    DetailUrl = Url.Action("Detail", "Destinations", new { id = d.Slug }) ?? "#",
                    BookingUrl = Url.Action("Create", "Bookings", new { destinationId = d.Id }) ?? "#"
                };
            }
            default:
                return null;
        }
    }

    private PendingBookingRowViewModel MapPendingBooking(Booking b, Payment? paid,
        CartBasketSegment segment, string myTripsUrl)
    {
        var confirmationUrl = Url.Action("Confirmation", "Bookings", new { id = b.Id }) ?? "#";

        PendingBookingRowViewModel row;

        if (b.DestinationId != null && b.Destination != null)
        {
            var d = b.Destination;
            var img = FirstNonEmpty(d.HeroImageUrl, d.CardImageUrl, d.ImageUrl);
            var rawPrice = string.IsNullOrWhiteSpace(d.CardPriceHint) ? d.PriceHint : d.CardPriceHint;

            row = new PendingBookingRowViewModel
            {
                BookingId = b.Id,
                KindBadge = "Destination",
                Title = d.Title,
                ImageUrl = img,
                PriceDisplayLine = CartPriceDisplay.PendingLine(rawPrice),
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Guests = b.Guests,
                DetailUrl = Url.Action("Detail", "Destinations", new { id = d.Slug }) ?? "#",
                ConfirmationUrl = confirmationUrl
            };
        }
        else if (b.ExperienceId != null && b.Experience != null)
        {
            var e = b.Experience;
            var img = FirstNonEmpty(e.ImageUrl, e.CardImageUrl);
            var price = string.IsNullOrWhiteSpace(e.CardPriceHint) ? e.PriceHint : e.CardPriceHint;

            row = new PendingBookingRowViewModel
            {
                BookingId = b.Id,
                KindBadge = "Experience",
                Title = e.Title,
                ImageUrl = img,
                PriceDisplayLine = CartPriceDisplay.PendingLine(price),
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Guests = b.Guests,
                DetailUrl = Url.Action("Detail", "Experiences", new { id = e.Slug }) ?? "#",
                ConfirmationUrl = confirmationUrl
            };
        }
        else if (b.TourId != null && b.Tour != null)
        {
            var t = b.Tour;
            var img = FirstNonEmpty(t.CardImageUrl, t.ImageUrl);
            var price = string.IsNullOrWhiteSpace(t.CardPriceHint)
                ? "$" + t.Price.ToString("0")
                : t.CardPriceHint;

            row = new PendingBookingRowViewModel
            {
                BookingId = b.Id,
                KindBadge = "Tour",
                Title = t.Title,
                ImageUrl = img,
                PriceDisplayLine = CartPriceDisplay.PendingLine(price),
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Guests = b.Guests,
                DetailUrl = Url.Action("Detail", "Tours", new { slug = t.Slug }) ?? "#",
                ConfirmationUrl = confirmationUrl
            };
        }
        else if (b.StayId != null && b.Stay != null)
        {
            var s = b.Stay;
            var img = FirstNonEmpty(s.CardImageUrl, s.ImageUrl);
            var price = string.IsNullOrWhiteSpace(s.CardPriceHint)
                ? "$" + s.PricePerNight.ToString("0") + " / night"
                : s.CardPriceHint;

            row = new PendingBookingRowViewModel
            {
                BookingId = b.Id,
                KindBadge = "Stay",
                Title = string.IsNullOrWhiteSpace(s.CardTitle) ? s.Name : s.CardTitle,
                ImageUrl = img,
                PriceDisplayLine = CartPriceDisplay.PendingLine(price),
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Guests = b.Guests,
                DetailUrl = Url.Action("Detail", "Hotels", new { id = s.Slug }) ?? "#",
                ConfirmationUrl = confirmationUrl
            };
        }
        else
        {
            row = new PendingBookingRowViewModel
            {
                BookingId = b.Id,
                KindBadge = "Request",
                Title = "Reservation request",
                ImageUrl = null,
                PriceDisplayLine = "Custom quote",
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                Guests = b.Guests,
                DetailUrl = Url.Action("Index", "Home") ?? "#",
                ConfirmationUrl = confirmationUrl
            };
        }

        ApplySegmentUi(b, paid, row, segment, myTripsUrl);
        return row;
    }

    private void ApplySegmentUi(Booking b, Payment? paid, PendingBookingRowViewModel row,
        CartBasketSegment segment, string myTripsUrl)
    {
        row.MyTripsUrl = myTripsUrl;
        row.CheckoutUrl = Url.Action("Checkout", "Payments", new { bookingId = b.Id }) ?? "#";

        switch (segment)
        {
            case CartBasketSegment.WaitingReview:
                row.HasPaid = false;
                row.PaidAmountUsd = null;
                row.ShowPayNow = false;
                row.ShowAwaitingQuote = false;
                row.StatusHeadline = "Waiting for concierge review";
                row.PaymentHint = "Payment opens after approval.";
                row.PaymentReceiptUrl = "#";
                break;
            case CartBasketSegment.ReadyPayment:
                row.HasPaid = false;
                row.PaidAmountUsd = null;
                var quote = BookingPaymentCalculator.TryGetPayableAmount(b);
                row.ShowPayNow = quote.CanPay && quote.Amount > 0;
                row.ShowAwaitingQuote = !(quote.CanPay && quote.Amount > 0);
                row.StatusHeadline = "Approved — payment available";
                row.PaymentHint = row.ShowPayNow
                    ? "Complete payment to secure your trip."
                    : "Awaiting concierge quote.";
                row.PaymentReceiptUrl = "#";
                break;
            case CartBasketSegment.PaidReceipt:
                row.HasPaid = true;
                row.PaidAmountUsd = paid?.Amount;
                row.ShowPayNow = false;
                row.ShowAwaitingQuote = false;
                row.StatusHeadline = "Payment received";
                row.PaymentHint = "Your trip is secured.";
                row.PaymentReceiptUrl = Url.Action("Success", "Payments", new { bookingId = b.Id }) ?? "#";
                break;
        }
    }

    private static string? FirstNonEmpty(params string?[] values)
    {
        foreach (var v in values)
        {
            if (!string.IsNullOrWhiteSpace(v))
                return v;
        }

        return null;
    }

    private enum CartBasketSegment
    {
        WaitingReview,
        ReadyPayment,
        PaidReceipt
    }
}
