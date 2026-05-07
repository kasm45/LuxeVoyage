using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Helpers;
using LuxeVoyage.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/stays")]
public class AdminStaysController : Controller
{
    private readonly ApplicationDbContext _db;
    public AdminStaysController(ApplicationDbContext db) => _db = db;

    [HttpGet("")]
    public async Task<IActionResult> Index(string? q, bool showInactive = false)
    {
        ViewBag.AdminPage = "Stays"; ViewData["Title"] = "Admin ? Stays"; ViewBag.SearchQuery = q; ViewBag.ShowInactive = showInactive;
        var query = _db.Stays.AsNoTracking().AsQueryable();
        if (showInactive) query = query.Where(s => !s.IsActive);
        else query = query.Where(s => s.IsActive);
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(s => s.Name.Contains(q) || s.Slug.Contains(q));
        return View(await query.OrderBy(s => s.Name).ToListAsync());
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var row = await _db.Stays.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Stays"; ViewData["Title"] = row.Name; return View(row);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewBag.AdminPage = "Stays"; ViewData["Title"] = "New stay";
        return View(new Stay { IsActive = true, IsVisibleOnListing = true });
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Stay model)
    {
        NormalizeAndValidateSlug(model);
        if (!ModelState.IsValid) { ViewBag.AdminPage = "Stays"; ViewData["Title"] = "New stay"; return View(model); }
        if (await SlugTakenAsync(model.Slug, null))
        {
            ModelState.AddModelError(nameof(model.Slug), "This slug is already in use. Choose a unique URL slug.");
            ViewBag.AdminPage = "Stays"; ViewData["Title"] = "New stay"; return View(model);
        }
        _db.Stays.Add(model); await _db.SaveChangesAsync(); TempData["Message"] = "Stay created."; return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var row = await _db.Stays.FindAsync(id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Stays"; ViewData["Title"] = "Edit stay"; return View(row);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Stay model)
    {
        if (id != model.Id) return BadRequest();
        NormalizeAndValidateSlug(model);
        if (!ModelState.IsValid) { ViewBag.AdminPage = "Stays"; ViewData["Title"] = "Edit stay"; return View(model); }
        if (await SlugTakenAsync(model.Slug, id))
        {
            ModelState.AddModelError(nameof(model.Slug), "This slug is already in use. Choose a unique URL slug.");
            ViewBag.AdminPage = "Stays"; ViewData["Title"] = "Edit stay"; return View(model);
        }
        var s = await _db.Stays.FindAsync(id); if (s == null) return NotFound();
        Copy(model, s); await _db.SaveChangesAsync(); TempData["Message"] = "Stay updated."; return RedirectToAction(nameof(Index));
    }

    [HttpPost("activate/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Activate(int id, string? q, bool showInactive = false)
    {
        var row = await _db.Stays.FindAsync(id); if (row == null) return NotFound();
        row.IsActive = true; await _db.SaveChangesAsync();
        TempData["Message"] = "Stay activated.";
        return RedirectToAction(nameof(Index), new { q, showInactive = false });
    }

    [HttpPost("deactivate/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deactivate(int id, string? q, bool showInactive = false)
    {
        var row = await _db.Stays.FindAsync(id); if (row == null) return NotFound();
        row.IsActive = false; await _db.SaveChangesAsync();
        TempData["Message"] = "Stay deactivated.";
        return RedirectToAction(nameof(Index), new { q, showInactive = true });
    }

    [HttpPost("delete-permanent/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeletePermanent(int id, string? q, bool showInactive = false)
    {
        var row = await _db.Stays.FindAsync(id); if (row == null) return NotFound();
        _db.Stays.Remove(row);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Stay permanently deleted.";
        return RedirectToAction(nameof(Index), new { q, showInactive });
    }

    private static void Copy(Stay s, Stay d)
    {
        d.Name = s.Name; d.Slug = s.Slug; d.StayType = s.StayType; d.PricePerNight = s.PricePerNight; d.StarRating = s.StarRating; d.Region = s.Region;
        d.CityLine = s.CityLine; d.ImageUrl = s.ImageUrl; d.AmenitiesCsv = s.AmenitiesCsv; d.IsActive = s.IsActive;
        d.CardTitle = s.CardTitle; d.CardSummary = s.CardSummary; d.CardImageUrl = s.CardImageUrl; d.CardBadge = s.CardBadge; d.CardRegion = s.CardRegion;
        d.CardPriceHint = s.CardPriceHint; d.CardRating = s.CardRating; d.IsVisibleOnListing = s.IsVisibleOnListing;
        d.HeroTitle = s.HeroTitle; d.HeroSubtitle = s.HeroSubtitle; d.HeroImageUrl = s.HeroImageUrl; d.DetailDescription = s.DetailDescription; d.DetailTagline = s.DetailTagline;
        d.AddressLabel = s.AddressLabel; d.RoomType = s.RoomType; d.GuestCapacity = s.GuestCapacity; d.BedInfo = s.BedInfo;
        d.AmenitiesDetailCsv = s.AmenitiesDetailCsv; d.HighlightsCsv = s.HighlightsCsv; d.NearbyAttractionsCsv = s.NearbyAttractionsCsv;
        d.CheckInInfo = s.CheckInInfo; d.CancellationPolicy = s.CancellationPolicy;
        d.GalleryImage1Url = s.GalleryImage1Url; d.GalleryImage2Url = s.GalleryImage2Url; d.GalleryImage3Url = s.GalleryImage3Url; d.GalleryImage4Url = s.GalleryImage4Url;
    }

    private void NormalizeAndValidateSlug(Stay model)
    {
        model.Slug = CatalogTextHelper.NormalizeSlug(model.Slug);
        if (string.IsNullOrEmpty(model.Slug)) ModelState.AddModelError(nameof(model.Slug), "Slug is required and must contain letters or numbers.");
    }

    private async Task<bool> SlugTakenAsync(string slug, int? excludeId)
    {
        var q = _db.Stays.AsNoTracking().Where(s => s.Slug == slug);
        if (excludeId is int id) q = q.Where(s => s.Id != id);
        return await q.AnyAsync();
    }
}
