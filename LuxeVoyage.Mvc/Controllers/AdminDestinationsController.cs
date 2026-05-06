using LuxeVoyage.Mvc.Data;
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
    public async Task<IActionResult> Index(string? q)
    {
        ViewBag.AdminPage = "Destinations";
        ViewData["Title"] = "Admin — Destinations";
        ViewBag.SearchQuery = q;
        var query = _db.Destinations.AsNoTracking().AsQueryable();
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
        return View(new Destination());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Destination model)
    {
        if (!ModelState.IsValid)
        {
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
        if (!ModelState.IsValid)
        {
            ViewBag.AdminPage = "Destinations";
            ViewData["Title"] = "Edit destination";
            return View(model);
        }

        _db.Entry(model).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        TempData["Message"] = "Destination updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.Destinations.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Destinations";
        ViewData["Title"] = "Delete destination";
        return View(row);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var row = await _db.Destinations.FindAsync(id);
        if (row == null) return NotFound();
        _db.Destinations.Remove(row);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Destination deleted.";
        return RedirectToAction(nameof(Index));
    }
}
