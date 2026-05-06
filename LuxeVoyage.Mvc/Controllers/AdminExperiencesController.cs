using LuxeVoyage.Mvc.Data;
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

    public AdminExperiencesController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string? q)
    {
        ViewBag.AdminPage = "Experiences";
        ViewData["Title"] = "Admin — Experiences";
        ViewBag.SearchQuery = q;
        var query = _db.Experiences.AsNoTracking().AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(e => e.Title.Contains(q) || e.Slug.Contains(q));
        return View(await query.OrderBy(e => e.Title).ToListAsync());
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var row = await _db.Experiences.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Experiences";
        ViewData["Title"] = $"Experience — {row.Title}";
        return View(row);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        ViewBag.AdminPage = "Experiences";
        ViewData["Title"] = "New experience";
        return View(new Experience());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Experience model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.AdminPage = "Experiences";
            ViewData["Title"] = "New experience";
            return View(model);
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
        if (!ModelState.IsValid)
        {
            ViewBag.AdminPage = "Experiences";
            ViewData["Title"] = "Edit experience";
            return View(model);
        }

        _db.Experiences.Update(model);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Experience updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.Experiences.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Experiences";
        ViewData["Title"] = "Delete experience";
        return View(row);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var row = await _db.Experiences.FindAsync(id);
        if (row == null) return NotFound();
        _db.Experiences.Remove(row);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Experience deleted.";
        return RedirectToAction(nameof(Index));
    }
}
