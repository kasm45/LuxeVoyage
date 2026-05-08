using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Helpers;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LuxeVoyage.Mvc.Controllers;

[Authorize(Roles = "Admin,Personnel")]
[Route("admin/reservations")]
public class AdminManageBookingsController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminManageBookingsController(ApplicationDbContext db) => _db = db;

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        ViewBag.AdminPage = "Bookings";
        ViewData["Title"] = "Admin — Reservations";
        var list = await _db.Bookings.AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .Where(b => b.Status == BookingStatus.Pending)
            .OrderByDescending(b => b.CreatedAtUtc)
            .ToListAsync();

        var ids = list.Select(b => b.Id).ToList();
        var paidList = await _db.Payments.AsNoTracking()
            .Where(p => ids.Contains(p.BookingId) && p.Status == PaymentStatus.Paid)
            .ToListAsync();
        var paidById = paidList
            .GroupBy(p => p.BookingId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.PaidAtUtc).First());

        var failedBookingIds = await _db.Payments.AsNoTracking()
            .Where(p => ids.Contains(p.BookingId) && p.Status == PaymentStatus.Failed)
            .Select(p => p.BookingId)
            .Distinct()
            .ToListAsync();
        var failedSet = failedBookingIds.ToHashSet();

        var rows = list.Select(b =>
        {
            paidById.TryGetValue(b.Id, out var paid);
            var est = BookingPaymentCalculator.TryGetEstimatedQuote(b);
            return new AdminPendingReservationRowVm
            {
                Booking = b,
                PaidPayment = paid,
                HasFailedPaymentAttempt = paid == null && failedSet.Contains(b.Id),
                QuoteAvailable = false,
                QuoteAmount = est.HasEstimate ? est.Amount : null
            };
        }).ToList();

        return View(rows);
    }

    [HttpGet("history")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> History(string? q, string status = "all")
    {
        ViewBag.AdminPage = "BookingsHistory";
        ViewData["Title"] = "Admin — Reservation History";
        ViewBag.SearchQuery = q;
        ViewBag.StatusFilter = status;

        var query = _db.Bookings.AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .Where(b => b.Status != BookingStatus.Pending);

        if (status.Equals("accepted", StringComparison.OrdinalIgnoreCase))
            query = query.Where(b => b.Status == BookingStatus.Accepted);
        else if (status.Equals("rejected", StringComparison.OrdinalIgnoreCase))
            query = query.Where(b => b.Status == BookingStatus.Rejected);
        else if (status.Equals("cancelled", StringComparison.OrdinalIgnoreCase))
            query = query.Where(b => b.Status == BookingStatus.Cancelled);
        else if (status.Equals("completed", StringComparison.OrdinalIgnoreCase))
            query = query.Where(b => b.Status == BookingStatus.Completed);

        if (!string.IsNullOrWhiteSpace(q))
        {
            query = query.Where(b =>
                (b.CustomerNameSnapshot != null && b.CustomerNameSnapshot.Contains(q)) ||
                (b.CustomerEmailSnapshot != null && b.CustomerEmailSnapshot.Contains(q)) ||
                (b.User != null && b.User.Email != null && b.User.Email.Contains(q)) ||
                (b.Tour != null && b.Tour.Title.Contains(q)) ||
                (b.Stay != null && b.Stay.Name.Contains(q)) ||
                (b.Experience != null && b.Experience.Title.Contains(q)) ||
                (b.Destination != null && b.Destination.Title.Contains(q)));
        }

        var list = await query
            .OrderByDescending(b => b.CancelledAtUtc ?? b.DecisionedAtUtc ?? b.CreatedAtUtc)
            .ToListAsync();

        var decidedByIds = list
            .Select(b => b.DecisionedByUserId)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Distinct()
            .ToList();

        var deciderDisplayLookup = await _db.Users.AsNoTracking()
            .Where(u => decidedByIds.Contains(u.Id))
            .ToDictionaryAsync(
                u => u.Id,
                u => !string.IsNullOrWhiteSpace(u.DisplayName)
                    ? u.DisplayName
                    : (u.Email ?? "Admin"));

        ViewBag.DeciderDisplayLookup = deciderDisplayLookup;
        return View(list);
    }

    [HttpPost("accept/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Accept(int id)
    {
        var row = await _db.Bookings
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (row == null) return NotFound();
        if (row.Status != BookingStatus.Pending)
        {
            TempData["Message"] = "Only pending reservations can be accepted.";
            return RedirectToAction(nameof(Index));
        }

        row.Status = BookingStatus.Accepted;
        row.DecisionedAtUtc = DateTime.UtcNow;
        row.DecisionedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _db.SaveChangesAsync();

        await CreateReservationNotificationAsync(
            row,
            "Your trip has been approved",
            "ReservationAccepted",
            "Your trip has been approved. Payment is now available for {0}.");

        TempData["Message"] = "Reservation accepted.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("reject/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        var row = await _db.Bookings
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (row == null) return NotFound();
        if (row.Status != BookingStatus.Pending)
        {
            TempData["Message"] = "Only pending reservations can be rejected.";
            return RedirectToAction(nameof(Index));
        }

        row.Status = BookingStatus.Rejected;
        row.DecisionedAtUtc = DateTime.UtcNow;
        row.DecisionedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _db.SaveChangesAsync();

        await CreateReservationNotificationAsync(
            row,
            "Reservation rejected",
            "ReservationRejected",
            "Your reservation request for {0} could not be accepted for the selected dates. Please choose another date range or contact our concierge team.");

        TempData["Message"] = "Reservation rejected.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("details/{id:int}")]
    public IActionResult Details(int id)
    {
        TempData["Message"] = "Use Accept/Reject from Reservations to manage requests.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        TempData["Message"] = "Use Accept/Reject from Reservations to manage requests.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Booking model)
    {
        TempData["Message"] = "Use Accept/Reject from Reservations to manage requests.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("delete/{id:int}")]
    public IActionResult Delete(int id)
    {
        TempData["Message"] = "Use Accept/Reject from Reservations to manage requests.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        TempData["Message"] = "Use Accept/Reject from Reservations to manage requests.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CreateReservationNotificationAsync(
        Booking booking,
        string title,
        string type,
        string messageTemplate)
    {
        var itemName = booking.Tour?.Title
            ?? booking.Stay?.Name
            ?? booking.Experience?.Title
            ?? booking.Destination?.Title
            ?? "your selected item";

        _db.Notifications.Add(new Notification
        {
            UserId = booking.UserId,
            Title = title,
            Message = string.Format(messageTemplate, itemName),
            Type = type,
            IsRead = false,
            CreatedAt = DateTime.UtcNow,
            ReservationId = booking.Id
        });
        await _db.SaveChangesAsync();
    }
}
