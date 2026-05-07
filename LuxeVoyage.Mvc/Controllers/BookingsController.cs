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
    public async Task<IActionResult> Create(int? tourId, int? stayId, int? experienceId, int? destinationId)
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
        {
            var returnUrl = Url.Action(nameof(Create), "Bookings",
                new { tourId, stayId, experienceId, destinationId })!;
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
            var d = await _db.Destinations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == destinationId);
            if (d != null)
            {
                model.DestinationId = d.Id;
                model.DestinationTitle = d.Title;
                model.BookingKind = "destination";
            }
        }

        if (model.TourId == null && model.StayId == null && model.ExperienceId == null &&
            model.DestinationId == null)
        {
            TempData["Error"] = "Select an experience, tour, or stay to book.";
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

        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Confirmation), new { id = booking.Id });
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

        ViewBag.NavSection = "Book";
        ViewData["Title"] = "Reservation received | LuxeVoyage";
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
            model.PriceHint = null;
            return;
        }

        ModelState.AddModelError(string.Empty, "Select an active listing before submitting a reservation.");
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
