using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Route("destinations")]
public class DestinationsController : Controller
{
    private readonly ApplicationDbContext _db;

    public DestinationsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string? category, string? region, bool expandFilters = false)
    {
        ViewBag.NavSection = "Destinations";
        ViewData["Title"] = "LuxeVoyage - Destinations";

        var q = _db.Destinations.AsNoTracking().AsQueryable();
        var cat = CatalogQueryHelper.ParseCategory(category);
        var reg = CatalogQueryHelper.ParseRegion(region);
        if (cat != null)
            q = q.Where(d => d.Category == cat.Value);
        if (reg != null)
            q = q.Where(d => d.Region == reg.Value);

        var rows = await q.OrderBy(d => d.Title).ToListAsync();

        HashSet<int> fav = new();
        var uid = GetUserId();
        if (!string.IsNullOrEmpty(uid))
        {
            var ids = rows.Select(r => r.Id).ToList();
            var favList = await _db.Favorites.AsNoTracking()
                .Where(f => f.UserId == uid && f.TargetKind == FavoriteTargetKind.Destination &&
                            ids.Contains(f.TargetId))
                .Select(f => f.TargetId)
                .ToListAsync();
            fav = favList.ToHashSet();
        }

        var cards = rows.Select(d => new AttractionCardVm
        {
            Id = d.Id,
            Slug = d.Slug,
            Title = d.Title,
            CategoryLabel = CatalogQueryHelper.CategoryDisplay(d.Category),
            RegionDisplay = CatalogQueryHelper.RegionDisplay(d.Region),
            ImageUrl = d.ImageUrl,
            Rating = d.Rating,
            Summary = d.Summary,
            IsFavorite = fav.Contains(d.Id)
        }).ToList();

        var vm = new AttractionsIndexViewModel
        {
            Headline = "Discover Extraordinary Destinations",
            Subtitle =
                "Explore handpicked destinations, cultural landmarks, and unforgettable places around the world.",
            SourceController = "Destinations",
            SourceArea = "destinations",
            Items = cards,
            Category = category,
            Region = region,
            ExpandFilters = expandFilters,
            TotalFilteredCount = cards.Count,
            CardKind = "destination"
        };

        return View(vm);
    }

    [HttpGet("filter")]
    public IActionResult Filter(string? category, string? region)
    {
        return RedirectToAction(nameof(Index), new { category, region });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Detail(string id)
    {
        var d = await _db.Destinations.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Slug == id);
        if (d == null && int.TryParse(id, System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture, out var numericId))
            d = await _db.Destinations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == numericId);

        if (d == null)
        {
            TempData["Error"] = "That destination could not be found.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.NavSection = "Destinations";
        ViewData["Title"] = $"LuxeVoyage - {d.Title}";

        var model = new ExperienceDetailViewModel
        {
            Id = d.Slug,
            NumericId = d.Id,
            Title = d.Title,
            BreadcrumbRegion = d.BreadcrumbRegion ?? CatalogQueryHelper.RegionDisplay(d.Region),
            BreadcrumbCity = d.BreadcrumbCity ?? "",
            BreadcrumbCurrent = d.BreadcrumbCurrent ?? d.Title,
            Location = d.LocationLabel ?? "",
            PriceDisplay = d.PriceHint ?? "",
            Summary = d.Summary,
            Rating = d.Rating,
            DetailKind = "destination"
        };

        return View(model);
    }

    private string? GetUserId() =>
        User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
}
