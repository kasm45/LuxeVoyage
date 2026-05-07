using System.Security.Claims;
using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Mapping;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Route("tours")]
public class ToursController : Controller
{
    private readonly ApplicationDbContext _db;
    private const int PageSize = 6;

    public ToursController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string sort = "popularity", int page = 1)
    {
        ViewBag.NavSection = "Tours";
        ViewData["Title"] = "Available Tours | LuxeVoyage";

        page = Math.Max(1, page);
        var all = await _db.Tours.AsNoTracking().Where(t => t.IsActive && t.IsVisibleOnListing).ToListAsync();

        List<Tour> ordered = sort switch
        {
            "priceAsc" => all.OrderBy(t => t.Price).ToList(),
            "priceDesc" => all.OrderByDescending(t => t.Price).ToList(),
            "durAsc" => all.OrderBy(t => t.DurationDays).ToList(),
            "durDesc" => all.OrderByDescending(t => t.DurationDays).ToList(),
            "ratingDesc" => all.OrderByDescending(t => t.CardRating ?? t.Rating).ThenByDescending(t => t.ReviewCount).ToList(),
            "newest" => all.OrderByDescending(t => t.Id).ToList(),
            _ => all.OrderByDescending(t => t.ReviewCount).ToList()
        };

        var total = ordered.Count;
        var totalPages = Math.Max(1, (int)Math.Ceiling(total / (double)PageSize));
        if (page > totalPages) page = totalPages;

        var slice = ordered.Skip((page - 1) * PageSize).Take(PageSize).ToList();

        HashSet<int> favTourIds = new();
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(uid) && slice.Count > 0)
        {
            var ids = slice.Select(t => t.Id).ToList();
            var favList = await _db.Favorites.AsNoTracking()
                .Where(f => f.UserId == uid && f.TargetKind == FavoriteTargetKind.Tour && ids.Contains(f.TargetId))
                .Select(f => f.TargetId)
                .ToListAsync();
            favTourIds = favList.ToHashSet();
        }

        var vm = new ToursIndexViewModel
        {
            Sort = sort,
            Page = page,
            TotalPages = totalPages,
            Tours = slice.Select(t => CatalogDisplayMapper.ToTourCard(t, favTourIds.Contains(t.Id))).ToList()
        };

        return View(vm);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return TourNotFound();

        slug = slug.Trim();
        var t = await _db.Tours.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug);
        if (t == null && int.TryParse(slug, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out var numericId))
            t = await _db.Tours.AsNoTracking().FirstOrDefaultAsync(x => x.Id == numericId);

        if (t == null || !t.IsActive) return TourNotFound();

        ViewBag.NavSection = "Tours";
        ViewData["Title"] = $"{t.Title} | LuxeVoyage";
        return View(CatalogDisplayMapper.ToTourDetail(t));
    }

    private IActionResult TourNotFound()
    {
        Response.StatusCode = 404;
        ViewBag.NavSection = "Tours";
        ViewData["Title"] = "Tour not found | LuxeVoyage";
        return View("NotFound");
    }
}
