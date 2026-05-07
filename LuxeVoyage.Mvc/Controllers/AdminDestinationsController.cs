using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Helpers;
using LuxeVoyage.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/destinations")]
public class AdminDestinationsController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminDestinationsController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string? q, bool showInactive = false)
    {
        ViewBag.AdminPage = "Destinations";
        ViewData["Title"] = "Admin — Destinations";
        ViewBag.SearchQuery = q;
        ViewBag.ShowInactive = showInactive;
        var query = _db.Destinations.AsNoTracking().AsQueryable();
        if (showInactive)
            query = query.Where(d => !d.IsActive);
        else
            query = query.Where(d => d.IsActive);
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(d => d.Title.Contains(q) || d.Slug.Contains(q));
        return View(await query.OrderBy(d => d.Title).ToListAsync());
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var row = await _db.Destinations.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Destinations";
        ViewData["Title"] = $"Destination — {row.Title}";
        return View(row);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewBag.AdminPage = "Destinations";
        ViewData["Title"] = "New destination";
        return View(new Destination { IsActive = true, IsVisibleOnListing = true });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Destination model)
    {
        NormalizeAndValidateSlug(model);
        if (!ModelState.IsValid)
        {
            ViewBag.AdminPage = "Destinations";
            ViewData["Title"] = "New destination";
            return View(model);
        }

        if (await SlugTakenAsync(model.Slug, null))
        {
            ModelState.AddModelError(nameof(model.Slug), "This slug is already in use. Choose a unique URL slug.");
            ViewBag.AdminPage = "Destinations";
            ViewData["Title"] = "New destination";
            return View(model);
        }

        _db.Destinations.Add(model);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Destination created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var row = await _db.Destinations.FindAsync(id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Destinations";
        ViewData["Title"] = "Edit destination";
        return View(row);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Destination model)
    {
        if (id != model.Id) return BadRequest();
        NormalizeAndValidateSlug(model);
        if (!ModelState.IsValid)
        {
            ViewBag.AdminPage = "Destinations";
            ViewData["Title"] = "Edit destination";
            return View(model);
        }

        if (await SlugTakenAsync(model.Slug, id))
        {
            ModelState.AddModelError(nameof(model.Slug), "This slug is already in use. Choose a unique URL slug.");
            ViewBag.AdminPage = "Destinations";
            ViewData["Title"] = "Edit destination";
            return View(model);
        }

        var existing = await _db.Destinations.FindAsync(id);
        if (existing == null) return NotFound();
        CopyDestination(model, existing);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Destination updated.";
        return RedirectToAction(nameof(Index));
    }

    private static void CopyDestination(Destination src, Destination dest)
    {
        dest.Title = src.Title;
        dest.Slug = src.Slug;
        dest.Category = src.Category;
        dest.Region = src.Region;
        dest.ImageUrl = src.ImageUrl;
        dest.Summary = src.Summary;
        dest.LocationLabel = src.LocationLabel;
        dest.Rating = src.Rating;
        dest.PriceHint = src.PriceHint;
        dest.BreadcrumbRegion = src.BreadcrumbRegion;
        dest.BreadcrumbCity = src.BreadcrumbCity;
        dest.BreadcrumbCurrent = src.BreadcrumbCurrent;
        dest.TagLine = src.TagLine;
        dest.HeroImageUrl = src.HeroImageUrl;
        dest.LongDescription = src.LongDescription;
        dest.BestTimeToVisit = src.BestTimeToVisit;
        dest.WeatherClimate = src.WeatherClimate;
        dest.WhereYoullBeText = src.WhereYoullBeText;
        dest.MapImageUrl = src.MapImageUrl;
        dest.GalleryImagesCsv = src.GalleryImagesCsv;
        dest.HighlightsCsv = src.HighlightsCsv;
        dest.IsActive = src.IsActive;
        dest.CardTitle = src.CardTitle;
        dest.CardSummary = src.CardSummary;
        dest.CardImageUrl = src.CardImageUrl;
        dest.CardBadge = src.CardBadge;
        dest.CardRegion = src.CardRegion;
        dest.CardPriceHint = src.CardPriceHint;
        dest.CardRating = src.CardRating;
        dest.IsVisibleOnListing = src.IsVisibleOnListing;
        dest.HeroTitle = src.HeroTitle;
        dest.HeroSubtitle = src.HeroSubtitle;
        dest.DetailDescription = src.DetailDescription;
        dest.DetailTagline = src.DetailTagline;
        dest.GalleryImage1Url = src.GalleryImage1Url;
        dest.GalleryImage2Url = src.GalleryImage2Url;
        dest.GalleryImage3Url = src.GalleryImage3Url;
        dest.GalleryImage4Url = src.GalleryImage4Url;
    }

    [HttpPost("activate/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(int id, string? q, bool showInactive = false)
    {
        var row = await _db.Destinations.FindAsync(id);
        if (row == null) return NotFound();
        row.IsActive = true;
        await _db.SaveChangesAsync();
        TempData["Message"] = "Destination activated.";
        return RedirectToAction(nameof(Index), new { q, showInactive = false });
    }

    [HttpPost("deactivate/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(int id, string? q, bool showInactive = false)
    {
        var row = await _db.Destinations.FindAsync(id);
        if (row == null) return NotFound();
        row.IsActive = false;
        await _db.SaveChangesAsync();
        TempData["Message"] = "Destination deactivated.";
        return RedirectToAction(nameof(Index), new { q, showInactive = true });
    }

    [HttpPost("delete-permanent/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePermanent(int id, string? q, bool showInactive = false)
    {
        var row = await _db.Destinations.FindAsync(id);
        if (row == null) return NotFound();
        _db.Destinations.Remove(row);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Destination permanently deleted.";
        return RedirectToAction(nameof(Index), new { q, showInactive });
    }

    private void NormalizeAndValidateSlug(Destination model)
    {
        model.Slug = CatalogTextHelper.NormalizeSlug(model.Slug);
        if (string.IsNullOrEmpty(model.Slug))
            ModelState.AddModelError(nameof(model.Slug), "Slug is required and must contain letters or numbers.");
    }

    private async Task<bool> SlugTakenAsync(string slug, int? excludeId)
    {
        var q = _db.Destinations.AsNoTracking().Where(d => d.Slug == slug);
        if (excludeId is int id)
            q = q.Where(d => d.Id != id);
        return await q.AnyAsync();
    }
}
