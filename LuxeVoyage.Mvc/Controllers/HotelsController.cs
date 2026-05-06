using System.Linq;
using LuxeVoyage.Mvc.Data;
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
    public async Task<IActionResult> Index(
        string price = "any",
        string stars = "any",
        string amenity = "any",
        string view = "grid")
    {
        ViewBag.NavSection = "Hotels";
        ViewData["Title"] = "Hotels - LuxeVoyage";

        var q = _db.Stays.AsNoTracking().AsQueryable();

        var tier = CatalogQueryHelper.ParsePriceTier(price);
        if (tier == "budget")
            q = q.Where(s => s.PricePerNight < 200);
        else if (tier == "mid")
            q = q.Where(s => s.PricePerNight >= 200 && s.PricePerNight <= 400);
        else if (tier == "luxury")
            q = q.Where(s => s.PricePerNight > 400);

        var starN = CatalogQueryHelper.ParseStars(stars);
        if (starN != null)
            q = q.Where(s => s.StarRating >= starN.Value);

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
                .Where(f => f.UserId == uid && f.TargetKind == FavoriteTargetKind.Stay &&
                            ids.Contains(f.TargetId))
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
            Stays = rows.Select(s => new StayCardVm
            {
                Id = s.Id,
                Name = s.Name,
                Slug = s.Slug,
                PricePerNight = s.PricePerNight,
                StarRating = s.StarRating,
                StarDisplay = s.StarRating,
                CityLine = s.CityLine,
                ImageUrl = s.ImageUrl,
                AmenityTags = s.AmenitiesCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(a => FormatAmenityLabel(a)).ToList(),
                IsFavorite = fav.Contains(s.Id)
            }).ToList()
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
        var s = await _db.Stays.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Slug == id);
        if (s == null && int.TryParse(id, System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture, out var numericId))
            s = await _db.Stays.AsNoTracking().FirstOrDefaultAsync(x => x.Id == numericId);

        if (s == null)
        {
            TempData["Error"] = "That stay could not be found.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.NavSection = "Hotels";
        ViewData["Title"] = $"LuxeVoyage - {s.Name}";

        var amenities = s.AmenitiesCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var summary = amenities.Length > 0
            ? $"Amenities include {string.Join(", ", amenities.Select(a => char.ToUpperInvariant(a[0]) + a[1..]))}."
            : $"Premium hospitality in {s.CityLine}.";

        var model = new ExperienceDetailViewModel
        {
            Id = s.Slug,
            NumericId = s.Id,
            Title = s.Name,
            BreadcrumbRegion = CatalogQueryHelper.RegionDisplay(s.Region),
            BreadcrumbCity = s.CityLine ?? "",
            BreadcrumbCurrent = s.Name,
            Location = s.CityLine ?? "",
            PriceDisplay = $"${s.PricePerNight.ToString("0")}",
            Summary = summary,
            Rating = s.StarRating,
            DetailKind = "stay"
        };

        return View(model);
    }

    private static string FormatAmenityLabel(string key)
    {
        if (string.IsNullOrEmpty(key)) return key;
        return char.ToUpperInvariant(key[0]) + key[1..];
    }
}
