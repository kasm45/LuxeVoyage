using LuxeVoyage.Mvc.Data;
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
    public async Task<IActionResult> Index(string? q)
    {
        ViewBag.AdminPage = "Stays";
        ViewData["Title"] = "Admin — Stays";
        ViewBag.SearchQuery = q;
        var query = _db.Stays.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(s => s.Name.Contains(q) || s.Slug.Contains(q));
        return View(await query.OrderBy(s => s.Name).ToListAsync());
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var row = await _db.Stays.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Stays";
        ViewData["Title"] = row.Name;
        return View(row);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewBag.AdminPage = "Stays";
        ViewData["Title"] = "New stay";
        return View(new Stay());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Stay model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.AdminPage = "Stays";
            return View(model);
        }
        _db.Stays.Add(model);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Stay created.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var row = await _db.Stays.FindAsync(id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Stays";
        ViewData["Title"] = "Edit stay";
        return View(row);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Stay model)
    {
        if (id != model.Id) return BadRequest();
        if (!ModelState.IsValid)
        {
            ViewBag.AdminPage = "Stays";
            return View(model);
        }
        _db.Stays.Update(model);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Stay updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.Stays.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Stays";
        return View(row);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var row = await _db.Stays.FindAsync(id);
        if (row == null) return NotFound();
        _db.Stays.Remove(row);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Stay deleted.";
        return RedirectToAction(nameof(Index));
    }
}
