using LuxeVoyage.Mvc.Data;
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
    public async Task<IActionResult> Index(string? q)
    {
        ViewBag.AdminPage = "Tours";
        ViewData["Title"] = "Admin — Tours";
        ViewBag.SearchQuery = q;
        var query = _db.Tours.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(t => t.Title.Contains(q) || t.Slug.Contains(q));
        return View(await query.OrderBy(t => t.Title).ToListAsync());
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var row = await _db.Tours.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Tours";
        ViewData["Title"] = row.Title;
        return View(row);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewBag.AdminPage = "Tours";
        ViewData["Title"] = "New tour";
        return View(new Tour());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Tour model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.AdminPage = "Tours";
            return View(model);
        }
        _db.Tours.Add(model);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Tour created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var row = await _db.Tours.FindAsync(id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Tours";
        ViewData["Title"] = "Edit tour";
        return View(row);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Tour model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.AdminPage = "Tours";
            return View(model);
        }
        _db.Tours.Update(model);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Tour updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.Tours.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Tours";
        return View(row);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var row = await _db.Tours.FindAsync(id);
        if (row == null) return NotFound();
        _db.Tours.Remove(row);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Tour deleted.";
        return RedirectToAction(nameof(Index));
    }
}
