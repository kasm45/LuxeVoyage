using System.Security.Claims;
using LuxeVoyage.Mvc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Authorize]
[Route("notifications")]
public class NotificationsController : Controller
{
    private readonly ApplicationDbContext _db;

    public NotificationsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        ViewData["Title"] = "Notifications | LuxeVoyage";
        ViewBag.NavSection = "Notifications";

        var items = await _db.Notifications.AsNoTracking()
            .Where(n => n.UserId == uid)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return View(items);
    }

    [HttpPost("mark-all-read")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAllRead()
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        var unread = await _db.Notifications
            .Where(n => n.UserId == uid && !n.IsRead)
            .ToListAsync();

        foreach (var n in unread)
            n.IsRead = true;

        if (unread.Count > 0)
            await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
