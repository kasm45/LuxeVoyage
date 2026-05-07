using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

/// <summary>Attribute routing required: app uses <c>MapControllers()</c> without a conventional route template.</summary>
[Route("[controller]/[action]")]
public class FavoritesController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<FavoritesController> _logger;
    private readonly IWebHostEnvironment _environment;

    public FavoritesController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        ILogger<FavoritesController> logger,
        IWebHostEnvironment environment)
    {
        _db = db;
        _userManager = userManager;
        _logger = logger;
        _environment = environment;
    }

    private IActionResult RedirectLoginOrReturn(string? returnUrl)
    {
        TempData["Error"] = "Please sign in to save favorites.";
        var ru = returnUrl;
        if (string.IsNullOrEmpty(ru))
            ru = $"{Request.Path}{Request.QueryString}";
        if (!Url.IsLocalUrl(ru))
            ru = Url.Content("~/")!;
        return RedirectToAction(nameof(AccountController.Login), "Account", new { returnUrl = ru });
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    private bool IsFavoriteFetchRequest() =>
        string.Equals(Request.Headers["X-Requested-With"], "Fetch", StringComparison.OrdinalIgnoreCase);

    private string ResolveLoginReturnUrl(string? returnUrl)
    {
        var ru = returnUrl;
        if (string.IsNullOrEmpty(ru))
            ru = $"{Request.Path}{Request.QueryString}";
        if (!Url.IsLocalUrl(ru))
            ru = Url.Content("~/")!;
        return ru;
    }

    private IActionResult LoginRequiredForAjax(string? returnUrl)
    {
        var ru = ResolveLoginReturnUrl(returnUrl);
        var loginUrl = Url.Action(nameof(AccountController.Login), "Account", new { returnUrl = ru })!;
        return StatusCode(401, new { ok = false, loginUrl });
    }

    /// <summary>
    /// itemType whitelist: Destination | Experience | Tour | Stay. itemId = DB primary key.
    /// Unique index (UserId, TargetKind, TargetId); <see cref="DbUpdateException"/> handled for races.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(string? itemType, int itemId, string? returnUrl)
    {
        var ajax = IsFavoriteFetchRequest();

        if (itemId <= 0)
        {
            if (ajax)
                return BadRequest(new { ok = false, error = "Invalid favorite request." });
            TempData["Error"] = "Invalid favorite request.";
            return RedirectToLocal(returnUrl);
        }

        FavoriteTargetKind? kind = itemType?.Trim() switch
        {
            "Destination" => FavoriteTargetKind.Destination,
            "Experience" => FavoriteTargetKind.Experience,
            "Stay" => FavoriteTargetKind.Stay,
            "Tour" => FavoriteTargetKind.Tour,
            _ => null
        };

        if (kind == null)
        {
            if (ajax)
                return BadRequest(new { ok = false, error = "Invalid favorite request." });
            TempData["Error"] = "Invalid favorite request.";
            return RedirectToLocal(returnUrl);
        }

        var authenticated = User.Identity?.IsAuthenticated == true;
        var userId = _userManager.GetUserId(User);

        if (string.IsNullOrEmpty(userId))
        {
            if (authenticated)
                return Unauthorized();
            if (ajax)
                return LoginRequiredForAjax(returnUrl);
            return RedirectLoginOrReturn(returnUrl);
        }

        if (_environment.IsDevelopment())
            _logger.LogInformation("Favorite toggle: user={userId}, itemId={ItemId}, type={ItemType}", userId, itemId, itemType);

        var existsForKind = await EntityExistsAsync(kind.Value, itemId);
        if (!existsForKind)
        {
            if (ajax)
                return BadRequest(new { ok = false, error = "That item could not be found." });
            TempData["Error"] = "That item could not be found.";
            return RedirectToLocal(returnUrl);
        }

        var existing = await _db.Favorites.FirstOrDefaultAsync(f =>
            f.UserId == userId &&
            f.TargetId == itemId &&
            f.TargetKind == kind.Value);

        var intendedRemove = existing != null;
        if (existing != null)
            _db.Favorites.Remove(existing);
        else
        {
            _db.Favorites.Add(new Favorite
            {
                UserId = userId,
                TargetKind = kind.Value,
                TargetId = itemId,
                CreatedAt = DateTime.UtcNow
            });
        }

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Race condition or duplicate favorite detected");
            _db.ChangeTracker.Clear();

            var resolved = await _db.Favorites.AsNoTracking()
                .FirstOrDefaultAsync(f =>
                    f.UserId == userId &&
                    f.TargetKind == kind.Value &&
                    f.TargetId == itemId);

            var nowFav = resolved != null;
            var raceMsg = nowFav ? "Saved to your favorites." : "Removed from your favorites.";

            if (_environment.IsDevelopment())
            {
                var count = await _db.Favorites.CountAsync();
                _logger.LogInformation("Total favorites in DB (after race handling): " + count);
            }

            if (ajax)
                return Json(new { ok = true, isFavorite = nowFav, message = raceMsg });

            TempData["Message"] = raceMsg;
            return RedirectToLocal(returnUrl);
        }

        if (_environment.IsDevelopment())
        {
            var count = await _db.Favorites.CountAsync();
            _logger.LogInformation("Total favorites in DB: " + count);
        }

        var nowFavorite = !intendedRemove;
        var message = nowFavorite ? "Saved to your favorites." : "Removed from your favorites.";

        if (ajax)
            return Json(new { ok = true, isFavorite = nowFavorite, message });

        TempData["Message"] = message;
        return RedirectToLocal(returnUrl);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> DebugFavorites()
    {
        if (!_environment.IsDevelopment())
            return NotFound();

        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
            return Json(new { error = "User not found" });

        var favorites = await _db.Favorites
            .AsNoTracking()
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => new
            {
                f.Id,
                f.TargetId,
                f.TargetKind,
                f.CreatedAt
            })
            .ToListAsync();

        return Json(new
        {
            count = favorites.Count,
            data = favorites
        });
    }

    private Task<bool> EntityExistsAsync(FavoriteTargetKind kind, int id) =>
        kind switch
        {
            FavoriteTargetKind.Experience => _db.Experiences.AsNoTracking().AnyAsync(e => e.Id == id && e.IsActive),
            FavoriteTargetKind.Destination => _db.Destinations.AsNoTracking().AnyAsync(d => d.Id == id),
            FavoriteTargetKind.Stay => _db.Stays.AsNoTracking().AnyAsync(s => s.Id == id && s.IsActive),
            FavoriteTargetKind.Tour => _db.Tours.AsNoTracking().AnyAsync(t => t.Id == id && t.IsActive),
            _ => Task.FromResult(false)
        };
}
