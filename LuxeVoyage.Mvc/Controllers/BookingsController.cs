using LuxeVoyage.Mvc.Data;
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
            var tour = await _db.Tours.AsNoTracking().FirstOrDefaultAsync(t => t.Id == tourId);
            if (tour != null)
            {
                model.TourId = tour.Id;
                model.TourTitle = tour.Title;
                model.PriceHint = tour.Price;
            }
        }

        if (stayId is > 0)
        {
            var stay = await _db.Stays.AsNoTracking().FirstOrDefaultAsync(s => s.Id == stayId);
            if (stay != null)
            {
                model.StayId = stay.Id;
                model.StayName = stay.Name;
                model.PriceHint = stay.PricePerNight;
            }
        }

        if (experienceId is > 0)
        {
            var ex = await _db.Experiences.AsNoTracking().FirstOrDefaultAsync(e => e.Id == experienceId);
            if (ex != null)
            {
                model.ExperienceId = ex.Id;
                model.ExperienceTitle = ex.Title;
            }
        }

        if (destinationId is > 0)
        {
            var d = await _db.Destinations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == destinationId);
            if (d != null)
            {
                model.DestinationId = d.Id;
                model.DestinationTitle = d.Title;
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

        if (model.EndDate < model.StartDate)
            ModelState.AddModelError(nameof(model.EndDate), "End date must be on or after the start date.");

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
}
