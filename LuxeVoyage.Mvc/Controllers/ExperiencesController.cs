using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Route("experiences")]
public class ExperiencesController : Controller
{
    private readonly ApplicationDbContext _db;

    public ExperiencesController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string? category, string? region, bool expandFilters = false)
    {
        ViewBag.NavSection = "Experiences";
        ViewData["Title"] = "LuxeVoyage - Experiences";

        var q = _db.Experiences.AsNoTracking().AsQueryable();
        var cat = CatalogQueryHelper.ParseCategory(category);
        var reg = CatalogQueryHelper.ParseRegion(region);
        if (cat != null)
            q = q.Where(e => e.Category == cat.Value);
        if (reg != null)
            q = q.Where(e => e.Region == reg.Value);

        var rows = await q.OrderBy(e => e.Title).ToListAsync();

        HashSet<int> fav = new();
        var uid = GetUserId();
        if (!string.IsNullOrEmpty(uid))
        {
            var ids = rows.Select(r => r.Id).ToList();
            var favList = await _db.Favorites.AsNoTracking()
                .Where(f => f.UserId == uid && f.TargetKind == FavoriteTargetKind.Experience &&
                            ids.Contains(f.TargetId))
                .Select(f => f.TargetId)
                .ToListAsync();
            fav = favList.ToHashSet();
        }

        var cards = rows.Select(e => new AttractionCardVm
        {
            Id = e.Id,
            Slug = e.Slug,
            Title = e.Title,
            CategoryLabel = CatalogQueryHelper.CategoryDisplay(e.Category),
            RegionDisplay = CatalogQueryHelper.RegionDisplay(e.Region),
            ImageUrl = e.ImageUrl,
            Rating = e.Rating,
            Summary = e.Summary,
            IsFavorite = fav.Contains(e.Id)
        }).ToList();

        var vm = new AttractionsIndexViewModel
        {
            Headline = "Discover Extraordinary Experiences",
            Subtitle = "From cultural discoveries to luxury adventures, find experiences designed to inspire every journey.",
            SourceController = "Experiences",
            SourceArea = "experiences",
            Items = cards,
            Category = category,
            Region = region,
            ExpandFilters = expandFilters,
            TotalFilteredCount = cards.Count,
            CardKind = "experience"
        };

        return View(vm);
    }

    /// <summary>Must be registered before <see cref="Detail"/> so "filter" is not captured as a slug.</summary>
    [HttpGet("filter")]
    public IActionResult Filter(string? category, string? region)
    {
        return RedirectToAction(nameof(Index), new { category, region });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Detail(string id)
    {
        var e = await _db.Experiences.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Slug == id);
        if (e == null && int.TryParse(id, System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture, out var numericId))
            e = await _db.Experiences.AsNoTracking().FirstOrDefaultAsync(x => x.Id == numericId);

        if (e == null)
        {
            TempData["Error"] = "That experience could not be found.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.NavSection = "Experiences";
        ViewData["Title"] = $"LuxeVoyage - {e.Title}";

        var model = new ExperienceDetailViewModel
        {
            Id = e.Slug,
            NumericId = e.Id,
            Title = e.Title,
            BreadcrumbRegion = e.BreadcrumbRegion ?? CatalogQueryHelper.RegionDisplay(e.Region),
            BreadcrumbCity = e.BreadcrumbCity ?? "",
            BreadcrumbCurrent = e.BreadcrumbCurrent ?? e.Title,
            Location = e.LocationLabel ?? "",
            PriceDisplay = e.PriceHint ?? "",
            Summary = e.Summary,
            Rating = e.Rating,
            DetailKind = "experience"
        };

        return View(model);
    }

    private string? GetUserId() =>
        User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
}
