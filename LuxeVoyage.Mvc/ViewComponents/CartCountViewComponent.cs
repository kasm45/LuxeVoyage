using System.Security.Claims;
using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.ViewComponents;

public class CartCountViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _db;

    public CartCountViewComponent(ApplicationDbContext db) => _db = db;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var uid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Content(string.Empty);

        var cartCount = await _db.CartItems.AsNoTracking()
            .CountAsync(c => c.UserId == uid && c.IsActive);

        var awaitingReview = await _db.Bookings.AsNoTracking()
            .CountAsync(b => b.UserId == uid && b.Status == BookingStatus.Pending);

        var acceptedIds = await _db.Bookings.AsNoTracking()
            .Where(b => b.UserId == uid && b.Status == BookingStatus.Accepted)
            .Select(b => b.Id)
            .ToListAsync();

        var paidBookingIds = await _db.Payments.AsNoTracking()
            .Where(p => p.Status == PaymentStatus.Paid && acceptedIds.Contains(p.BookingId))
            .Select(p => p.BookingId)
            .Distinct()
            .ToListAsync();

        var paidSet = paidBookingIds.ToHashSet();
        var paymentDue = acceptedIds.Count(id => !paidSet.Contains(id));

        var total = cartCount + awaitingReview + paymentDue;

        return View(total);
    }
}
