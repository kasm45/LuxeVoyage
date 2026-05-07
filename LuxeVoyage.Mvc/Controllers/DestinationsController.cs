using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Mapping;
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

        var q = _db.Destinations.AsNoTracking()
            .Where(d => d.IsActive && d.IsVisibleOnListing).AsQueryable();
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

        var cards = rows.Select(d => DestinationDisplayMapper.ToListingCard(d, fav.Contains(d.Id))).ToList();

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

        if (d == null || !d.IsActive)
            return DestinationNotFound();

        ViewBag.NavSection = "Destinations";
        ViewData["Title"] = $"LuxeVoyage - {DestinationDisplayMapper.EffectiveHeroTitle(d)}";

        var model = DestinationDisplayMapper.ToDetail(d);
        var destinationCity = ResolveDestinationCity(d);
        if (!string.IsNullOrWhiteSpace(destinationCity))
        {
            var topAttractions = await _db.Attractions.AsNoTracking()
                .Where(a => a.City == destinationCity)
                .OrderBy(a => a.Name)
                .Take(4)
                .Select(a => new DestinationAttractionViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    City = a.City,
                    Category = a.Category,
                    ImageUrl = a.ImageUrl
                })
                .ToListAsync();
            model.TopAttractions = topAttractions;
        }

        return View(model);
    }

    private IActionResult DestinationNotFound()
    {
        Response.StatusCode = 404;
        ViewBag.NavSection = "Destinations";
        ViewData["Title"] = "Destination not found | LuxeVoyage";
        return View("NotFound");
    }

    private string? GetUserId() =>
        User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    private static string? ResolveDestinationCity(Destination destination)
    {
        var fromBreadcrumb = destination.BreadcrumbCity?.Trim();
        if (!string.IsNullOrWhiteSpace(fromBreadcrumb))
            return fromBreadcrumb;

        var fromLocation = destination.LocationLabel?.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(fromLocation))
            return fromLocation;

        var fromTitle = destination.Title.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(fromTitle))
            return fromTitle;

        return null;
    }
}
