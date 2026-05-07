using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/attractions")]
public class AdminAttractionsController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminAttractionsController(ApplicationDbContext db) => _db = db;

    [HttpGet("")]
    public async Task<IActionResult> Index(string? q)
    {
        ViewBag.AdminPage = "Attractions";
        ViewData["Title"] = "Admin Panel - Manage Points of Interest";
        ViewBag.SearchQuery = q;
        var query = _db.Attractions.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(a => a.Name.Contains(q) || a.City.Contains(q) || a.Category.Contains(q));
        var list = await query
            .OrderBy(a => a.Name)
            .ToListAsync();

        var cityKeys = list
            .Select(a => a.City?.Trim())
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var destinationRows = await _db.Destinations.AsNoTracking()
            .Where(d => d.IsActive)
            .Select(d => new { d.Slug, d.Title, d.BreadcrumbCity, d.LocationLabel })
            .ToListAsync();

        var previewByCity = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var city in cityKeys)
        {
            var c = city!;
            var match = destinationRows.FirstOrDefault(d =>
                (!string.IsNullOrWhiteSpace(d.BreadcrumbCity) && d.BreadcrumbCity.Equals(c, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(d.Title) && d.Title.StartsWith(c, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrWhiteSpace(d.LocationLabel) && d.LocationLabel.StartsWith(c, StringComparison.OrdinalIgnoreCase)));

            previewByCity[c] = match != null
                ? Url.Action("Detail", "Destinations", new { id = match.Slug }) ?? "/destinations"
                : "/destinations";
        }

        ViewBag.PreviewByCity = previewByCity;
        return View(list);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewBag.AdminPage = "Attractions";
        ViewData["Title"] = "New point of interest";
        return View(new Attraction());
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var row = await _db.Attractions.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        if (row == null)
        {
            TempData["Error"] = "Point of interest not found.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.AdminPage = "Attractions";
        ViewData["Title"] = row.Name;
        return View(row);
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Attraction model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.AdminPage = "Attractions";
            ViewData["Title"] = "New point of interest";
            return View(model);
        }

        _db.Attractions.Add(model);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Point of interest created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var row = await _db.Attractions.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        if (row == null)
        {
            TempData["Error"] = "Point of interest not found.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.AdminPage = "Attractions";
        ViewData["Title"] = "Edit point of interest";
        return View(row);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Attraction model)
    {
        if (id != model.Id)
        {
            TempData["Error"] = "Invalid point of interest.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            ViewBag.AdminPage = "Attractions";
            ViewData["Title"] = "Edit point of interest";
            return View(model);
        }

        var existing = await _db.Attractions.FindAsync(id);
        if (existing == null)
        {
            TempData["Error"] = "Point of interest not found.";
            return RedirectToAction(nameof(Index));
        }

        existing.Name = model.Name;
        existing.City = model.City;
        existing.Category = model.Category;
        existing.ImageUrl = model.ImageUrl;
        await _db.SaveChangesAsync();
        TempData["Message"] = "Point of interest updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.Attractions.FindAsync(id);
        if (row == null)
        {
            TempData["Error"] = "Point of interest not found.";
            return RedirectToAction(nameof(Index));
        }

        _db.Attractions.Remove(row);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Point of interest deleted.";
        return RedirectToAction(nameof(Index));
    }
}
