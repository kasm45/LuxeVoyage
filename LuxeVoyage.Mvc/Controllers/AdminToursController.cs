using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Helpers;
using LuxeVoyage.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/tours")]
public class AdminToursController : Controller
{
    private readonly ApplicationDbContext _db;
    public AdminToursController(ApplicationDbContext db) => _db = db;

    [HttpGet("")]
    public async Task<IActionResult> Index(string? q, bool showInactive = false)
    {
        ViewBag.AdminPage = "Tours"; ViewData["Title"] = "Admin ? Tours"; ViewBag.SearchQuery = q; ViewBag.ShowInactive = showInactive;
        var query = _db.Tours.AsNoTracking().AsQueryable();
        if (showInactive) query = query.Where(t => !t.IsActive);
        else query = query.Where(t => t.IsActive);
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(t => t.Title.Contains(q) || t.Slug.Contains(q));
        return View(await query.OrderBy(t => t.Title).ToListAsync());
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var row = await _db.Tours.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Tours"; ViewData["Title"] = row.Title; return View(row);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewBag.AdminPage = "Tours"; ViewData["Title"] = "New tour";
        return View(new Tour { IsActive = true, IsVisibleOnListing = true });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tour model)
    {
        NormalizeAndValidateSlug(model);
        if (!ModelState.IsValid) { ViewBag.AdminPage = "Tours"; ViewData["Title"] = "New tour"; return View(model); }
        if (await SlugTakenAsync(model.Slug, null))
        {
            ModelState.AddModelError(nameof(model.Slug), "This slug is already in use. Choose a unique URL slug.");
            ViewBag.AdminPage = "Tours"; ViewData["Title"] = "New tour"; return View(model);
        }
        _db.Tours.Add(model); await _db.SaveChangesAsync(); TempData["Message"] = "Tour created."; return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var row = await _db.Tours.FindAsync(id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Tours"; ViewData["Title"] = "Edit tour"; return View(row);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Tour model)
    {
        if (id != model.Id) return BadRequest();
        NormalizeAndValidateSlug(model);
        if (!ModelState.IsValid) { ViewBag.AdminPage = "Tours"; ViewData["Title"] = "Edit tour"; return View(model); }
        if (await SlugTakenAsync(model.Slug, id))
        {
            ModelState.AddModelError(nameof(model.Slug), "This slug is already in use. Choose a unique URL slug.");
            ViewBag.AdminPage = "Tours"; ViewData["Title"] = "Edit tour"; return View(model);
        }
        var t = await _db.Tours.FindAsync(id); if (t == null) return NotFound();
        Copy(model, t); await _db.SaveChangesAsync(); TempData["Message"] = "Tour updated."; return RedirectToAction(nameof(Index));
    }

    [HttpPost("activate/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(int id, string? q, bool showInactive = false)
    {
        var row = await _db.Tours.FindAsync(id); if (row == null) return NotFound();
        row.IsActive = true; await _db.SaveChangesAsync();
        TempData["Message"] = "Tour activated.";
        return RedirectToAction(nameof(Index), new { q, showInactive = false });
    }

    [HttpPost("deactivate/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(int id, string? q, bool showInactive = false)
    {
        var row = await _db.Tours.FindAsync(id); if (row == null) return NotFound();
        row.IsActive = false; await _db.SaveChangesAsync();
        TempData["Message"] = "Tour deactivated.";
        return RedirectToAction(nameof(Index), new { q, showInactive = true });
    }

    [HttpPost("delete-permanent/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePermanent(int id, string? q, bool showInactive = false)
    {
        var row = await _db.Tours.FindAsync(id); if (row == null) return NotFound();
        _db.Tours.Remove(row);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Tour permanently deleted.";
        return RedirectToAction(nameof(Index), new { q, showInactive });
    }

    private static void Copy(Tour s, Tour d)
    {
        d.Title = s.Title; d.Slug = s.Slug; d.Region = s.Region; d.Price = s.Price; d.DurationDays = s.DurationDays;
        d.CategoryLabel = s.CategoryLabel; d.ImageUrl = s.ImageUrl; d.Summary = s.Summary; d.Rating = s.Rating; d.ReviewCount = s.ReviewCount;
        d.IsActive = s.IsActive; d.CardTitle = s.CardTitle; d.CardSummary = s.CardSummary; d.CardImageUrl = s.CardImageUrl; d.CardBadge = s.CardBadge;
        d.CardRegion = s.CardRegion; d.CardPriceHint = s.CardPriceHint; d.CardRating = s.CardRating; d.IsVisibleOnListing = s.IsVisibleOnListing;
        d.HeroTitle = s.HeroTitle; d.HeroSubtitle = s.HeroSubtitle; d.HeroImageUrl = s.HeroImageUrl; d.DetailDescription = s.DetailDescription;
        d.DetailTagline = s.DetailTagline; d.DestinationLabel = s.DestinationLabel; d.GroupSizeText = s.GroupSizeText;
        d.IncludedItemsCsv = s.IncludedItemsCsv; d.HighlightsCsv = s.HighlightsCsv; d.ItineraryText = s.ItineraryText;
        d.WhatToBring = s.WhatToBring; d.CancellationPolicy = s.CancellationPolicy;
        d.GalleryImage1Url = s.GalleryImage1Url; d.GalleryImage2Url = s.GalleryImage2Url; d.GalleryImage3Url = s.GalleryImage3Url; d.GalleryImage4Url = s.GalleryImage4Url;
    }

    private void NormalizeAndValidateSlug(Tour model)
    {
        model.Slug = CatalogTextHelper.NormalizeSlug(model.Slug);
        if (string.IsNullOrEmpty(model.Slug)) ModelState.AddModelError(nameof(model.Slug), "Slug is required and must contain letters or numbers.");
    }

    private async Task<bool> SlugTakenAsync(string slug, int? excludeId)
    {
        var q = _db.Tours.AsNoTracking().Where(t => t.Slug == slug);
        if (excludeId is int id) q = q.Where(t => t.Id != id);
        return await q.AnyAsync();
    }
}
