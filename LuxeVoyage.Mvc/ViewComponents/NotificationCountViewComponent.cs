using System.Security.Claims;

using LuxeVoyage.Mvc.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.ViewComponents;

/// <summary>
/// Unread notification badge count for the navbar (same query logic as before; moved out of the Razor view).
/// </summary>
public class NotificationCountViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _db;

    public NotificationCountViewComponent(ApplicationDbContext db) => _db = db;

    /// <param name="variant">
    /// <c>rail</c> matches desktop / compact mobile icon strip; <c>drawer</c> matches the slightly larger pill in the mobile menu drawer.
    /// </param>
    public async Task<IViewComponentResult> InvokeAsync(string variant = "rail")
    {
        var uid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Content(string.Empty);

        var count = await _db.Notifications.AsNoTracking()
            .CountAsync(n => n.UserId == uid && !n.IsRead);

        if (string.Equals(variant, "drawer", StringComparison.OrdinalIgnoreCase))
            return View("Drawer", count);

        return View(count);
    }
}
