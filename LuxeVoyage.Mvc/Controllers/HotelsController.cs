using System.Linq;
using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Mapping;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Route("hotels")]
[Route("stays")]
public class HotelsController : Controller
{
    private readonly ApplicationDbContext _db;

    public HotelsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string price = "any", string stars = "any", string amenity = "any", string view = "grid")
    {
        ViewBag.NavSection = "Hotels";
        ViewData["Title"] = "Hotels - LuxeVoyage";

        var q = _db.Stays.AsNoTracking().Where(s => s.IsActive && s.IsVisibleOnListing).AsQueryable();

        var tier = CatalogQueryHelper.ParsePriceTier(price);
        if (tier == "budget") q = q.Where(s => s.PricePerNight < 200);
        else if (tier == "mid") q = q.Where(s => s.PricePerNight >= 200 && s.PricePerNight <= 400);
        else if (tier == "luxury") q = q.Where(s => s.PricePerNight > 400);

        var starN = CatalogQueryHelper.ParseStars(stars);
        if (starN != null) q = q.Where(s => s.StarRating >= starN.Value);

        var am = CatalogQueryHelper.ParseAmenity(amenity);
        if (!string.IsNullOrEmpty(am))
        {
            var amLower = am.ToLowerInvariant();
            q = q.Where(s => s.AmenitiesCsv.ToLower().Contains(amLower));
        }

        var rows = await q.OrderBy(s => s.Name).ToListAsync();

        HashSet<int> fav = new();
        var uid = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(uid))
        {
            var ids = rows.Select(r => r.Id).ToList();
            var favList = await _db.Favorites.AsNoTracking()
                .Where(f => f.UserId == uid && f.TargetKind == FavoriteTargetKind.Stay && ids.Contains(f.TargetId))
                .Select(f => f.TargetId)
                .ToListAsync();
            fav = favList.ToHashSet();
        }

        var vm = new HotelsIndexViewModel
        {
            PriceFilter = string.IsNullOrEmpty(tier) ? (string.IsNullOrWhiteSpace(price) || price.Equals("any", StringComparison.OrdinalIgnoreCase) ? "any" : price.Trim().ToLowerInvariant()) : tier,
            StarsFilter = starN?.ToString() ?? (string.IsNullOrWhiteSpace(stars) || stars.Equals("any", StringComparison.OrdinalIgnoreCase) ? "any" : stars.Trim()),
            AmenityFilter = am ?? (string.IsNullOrWhiteSpace(amenity) || amenity.Equals("any", StringComparison.OrdinalIgnoreCase) ? "any" : amenity.Trim().ToLowerInvariant()),
            ViewMode = view is "map" or "grid" ? view : "grid",
            TotalCount = rows.Count,
            Stays = rows.Select(s => CatalogDisplayMapper.ToStayCard(s, fav.Contains(s.Id))).ToList()
        };

        return View(vm);
    }

    [HttpGet("filters")]
    public IActionResult Filters(string price, string stars, string amenity, string view)
    {
        return RedirectToAction(nameof(Index), new { price, stars, amenity, view });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Detail(string id)
    {
        var s = await _db.Stays.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == id);
        if (s == null && int.TryParse(id, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var numericId))
            s = await _db.Stays.AsNoTracking().FirstOrDefaultAsync(x => x.Id == numericId);

        if (s == null || !s.IsActive) return StayNotFound();

        ViewBag.NavSection = "Hotels";
        ViewData["Title"] = $"LuxeVoyage - {s.Name}";
        return View(CatalogDisplayMapper.ToStayDetail(s));
    }

    private IActionResult StayNotFound()
    {
        Response.StatusCode = 404;
        ViewBag.NavSection = "Hotels";
        ViewData["Title"] = "Stay not found | LuxeVoyage";
        return View("NotFound");
    }
}
