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
        ViewData["Title"] = "Admin Panel - Attraction Management";
        ViewBag.SearchQuery = q;
        var query = _db.Attractions.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(a => a.Name.Contains(q) || a.City.Contains(q) || a.Category.Contains(q));
        var list = await query
            .OrderBy(a => a.Name)
            .ToListAsync();
        return View(list);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewBag.AdminPage = "Attractions";
        ViewData["Title"] = "New attraction";
        return View(new Attraction());
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var row = await _db.Attractions.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        if (row == null)
        {
            TempData["Error"] = "Attraction not found.";
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
            ViewData["Title"] = "New attraction";
            return View(model);
        }

        _db.Attractions.Add(model);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Attraction created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var row = await _db.Attractions.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        if (row == null)
        {
            TempData["Error"] = "Attraction not found.";
            return RedirectToAction(nameof(Index));
        }

        ViewBag.AdminPage = "Attractions";
        ViewData["Title"] = "Edit attraction";
        return View(row);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Attraction model)
    {
        if (id != model.Id)
        {
            TempData["Error"] = "Invalid attraction.";
            return RedirectToAction(nameof(Index));
        }

        if (!ModelState.IsValid)
        {
            ViewBag.AdminPage = "Attractions";
            ViewData["Title"] = "Edit attraction";
            return View(model);
        }

        var existing = await _db.Attractions.FindAsync(id);
        if (existing == null)
        {
            TempData["Error"] = "Attraction not found.";
            return RedirectToAction(nameof(Index));
        }

        existing.Name = model.Name;
        existing.City = model.City;
        existing.Category = model.Category;
        existing.ImageUrl = model.ImageUrl;
        await _db.SaveChangesAsync();
        TempData["Message"] = "Attraction updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.Attractions.FindAsync(id);
        if (row == null)
        {
            TempData["Error"] = "Attraction not found.";
            return RedirectToAction(nameof(Index));
        }

        _db.Attractions.Remove(row);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Attraction deleted.";
        return RedirectToAction(nameof(Index));
    }
}
