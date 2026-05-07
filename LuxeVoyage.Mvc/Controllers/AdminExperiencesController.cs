using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Helpers;
using LuxeVoyage.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/experiences")]
public class AdminExperiencesController : Controller
{
    private readonly ApplicationDbContext _db;
    public AdminExperiencesController(ApplicationDbContext db) => _db = db;

    [HttpGet("")]
    public async Task<IActionResult> Index(string? q, bool showInactive = false)
    {
        ViewBag.AdminPage = "Experiences";
        ViewData["Title"] = "Admin ? Experiences";
        ViewBag.SearchQuery = q;
        ViewBag.ShowInactive = showInactive;
        var query = _db.Experiences.AsNoTracking().AsQueryable();
        if (showInactive) query = query.Where(e => !e.IsActive);
        else query = query.Where(e => e.IsActive);
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(e => e.Title.Contains(q) || e.Slug.Contains(q));
        return View(await query.OrderBy(e => e.Title).ToListAsync());
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var row = await _db.Experiences.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Experiences";
        ViewData["Title"] = $"Experience ? {row.Title}";
        return View(row);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewBag.AdminPage = "Experiences";
        ViewData["Title"] = "New experience";
        return View(new Experience { IsActive = true, IsVisibleOnListing = true });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Experience model)
    {
        NormalizeAndValidateSlug(model);
        if (!ModelState.IsValid) { ViewBag.AdminPage = "Experiences"; ViewData["Title"] = "New experience"; return View(model); }
        if (await SlugTakenAsync(model.Slug, null))
        {
            ModelState.AddModelError(nameof(model.Slug), "This slug is already in use. Choose a unique URL slug.");
            ViewBag.AdminPage = "Experiences"; ViewData["Title"] = "New experience"; return View(model);
        }
        _db.Experiences.Add(model);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Experience created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var row = await _db.Experiences.FindAsync(id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Experiences";
        ViewData["Title"] = "Edit experience";
        return View(row);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Experience model)
    {
        if (id != model.Id) return BadRequest();
        NormalizeAndValidateSlug(model);
        if (!ModelState.IsValid) { ViewBag.AdminPage = "Experiences"; ViewData["Title"] = "Edit experience"; return View(model); }
        if (await SlugTakenAsync(model.Slug, id))
        {
            ModelState.AddModelError(nameof(model.Slug), "This slug is already in use. Choose a unique URL slug.");
            ViewBag.AdminPage = "Experiences"; ViewData["Title"] = "Edit experience"; return View(model);
        }

        var e = await _db.Experiences.FindAsync(id);
        if (e == null) return NotFound();
        Copy(model, e);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Experience updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("activate/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(int id, string? q, bool showInactive = false)
    {
        var row = await _db.Experiences.FindAsync(id);
        if (row == null) return NotFound();
        row.IsActive = true;
        await _db.SaveChangesAsync();
        TempData["Message"] = "Experience activated.";
        return RedirectToAction(nameof(Index), new { q, showInactive = false });
    }

    [HttpPost("deactivate/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(int id, string? q, bool showInactive = false)
    {
        var row = await _db.Experiences.FindAsync(id);
        if (row == null) return NotFound();
        row.IsActive = false;
        await _db.SaveChangesAsync();
        TempData["Message"] = "Experience deactivated.";
        return RedirectToAction(nameof(Index), new { q, showInactive = true });
    }

    [HttpPost("delete-permanent/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePermanent(int id, string? q, bool showInactive = false)
    {
        var row = await _db.Experiences.FindAsync(id);
        if (row == null) return NotFound();
        _db.Experiences.Remove(row);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Experience permanently deleted.";
        return RedirectToAction(nameof(Index), new { q, showInactive });
    }

    private static void Copy(Experience s, Experience d)
    {
        d.Title = s.Title; d.Slug = s.Slug; d.Category = s.Category; d.Region = s.Region; d.ImageUrl = s.ImageUrl; d.Summary = s.Summary;
        d.LocationLabel = s.LocationLabel; d.Rating = s.Rating; d.PriceHint = s.PriceHint; d.BreadcrumbRegion = s.BreadcrumbRegion;
        d.BreadcrumbCity = s.BreadcrumbCity; d.BreadcrumbCurrent = s.BreadcrumbCurrent; d.TagLine = s.TagLine; d.IsActive = s.IsActive;
        d.CardTitle = s.CardTitle; d.CardSummary = s.CardSummary; d.CardImageUrl = s.CardImageUrl; d.CardBadge = s.CardBadge;
        d.CardRegion = s.CardRegion; d.CardPriceHint = s.CardPriceHint; d.CardRating = s.CardRating; d.IsVisibleOnListing = s.IsVisibleOnListing;
        d.HeroTitle = s.HeroTitle; d.HeroSubtitle = s.HeroSubtitle; d.HeroImageUrl = s.HeroImageUrl; d.DetailDescription = s.DetailDescription;
        d.DetailTagline = s.DetailTagline; d.DurationText = s.DurationText; d.GroupSizeText = s.GroupSizeText; d.MeetingPoint = s.MeetingPoint;
        d.IncludedItemsCsv = s.IncludedItemsCsv; d.HighlightsCsv = s.HighlightsCsv; d.ItineraryText = s.ItineraryText;
        d.GalleryImage1Url = s.GalleryImage1Url; d.GalleryImage2Url = s.GalleryImage2Url; d.GalleryImage3Url = s.GalleryImage3Url; d.GalleryImage4Url = s.GalleryImage4Url;
    }

    private void NormalizeAndValidateSlug(Experience model)
    {
        model.Slug = CatalogTextHelper.NormalizeSlug(model.Slug);
        if (string.IsNullOrEmpty(model.Slug)) ModelState.AddModelError(nameof(model.Slug), "Slug is required and must contain letters or numbers.");
    }

    private async Task<bool> SlugTakenAsync(string slug, int? excludeId)
    {
        var q = _db.Experiences.AsNoTracking().Where(e => e.Slug == slug);
        if (excludeId is int id) q = q.Where(e => e.Id != id);
        return await q.AnyAsync();
    }
}
