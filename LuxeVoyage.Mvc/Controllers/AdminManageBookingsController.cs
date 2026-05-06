using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin/reservations")]
public class AdminManageBookingsController : Controller
{
    private readonly ApplicationDbContext _db;

    public AdminManageBookingsController(ApplicationDbContext db) => _db = db;

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        ViewBag.AdminPage = "Bookings";
        ViewData["Title"] = "Admin — Reservations";
        var list = await _db.Bookings.AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .OrderByDescending(b => b.CreatedAtUtc)
            .ToListAsync();
        return View(list);
    }

    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var row = await _db.Bookings
            .Include(b => b.User)
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Bookings";
        ViewData["Title"] = $"Booking #{row.Id}";
        return View(row);
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var row = await _db.Bookings.FindAsync(id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Bookings";
        ViewData["Title"] = "Edit reservation";
        return View(row);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Booking model)
    {
        if (id != model.Id) return BadRequest();
        var row = await _db.Bookings.FindAsync(id);
        if (row == null) return NotFound();

        row.Status = model.Status;
        row.StartDate = model.StartDate.Date;
        row.EndDate = model.EndDate.Date;
        row.Guests = model.Guests;
        row.Notes = model.Notes;
        await _db.SaveChangesAsync();
        TempData["Message"] = "Reservation updated.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var row = await _db.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        if (row == null) return NotFound();
        ViewBag.AdminPage = "Bookings";
        return View(row);
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var row = await _db.Bookings.FindAsync(id);
        if (row == null) return NotFound();
        _db.Bookings.Remove(row);
        await _db.SaveChangesAsync();
        TempData["Message"] = "Reservation deleted.";
        return RedirectToAction(nameof(Index));
    }
}
