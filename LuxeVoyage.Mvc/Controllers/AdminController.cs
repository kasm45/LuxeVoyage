using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LuxeVoyage.Mvc.Controllers;

[Authorize(Roles = "Admin,Personnel")]
[Route("admin")]
public class AdminController : Controller
{
    private const string ArchiveUserEmail = "archived.user@luxevoyage.local";
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWebHostEnvironment _env;

    public AdminController(
        ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        IWebHostEnvironment env)
    {
        _db = db;
        _userManager = userManager;
        _env = env;
    }

    [HttpGet("")]
    public IActionResult Dashboard()
    {
        if (!User.IsInRole("Admin"))
            return RedirectToAction(nameof(AdminManageBookingsController.Index), "AdminManageBookings");

        ViewBag.AdminPage = "Dashboard";
        ViewData["Title"] = "LuxeVoyage Admin Dashboard";
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> Users()
    {
        ViewBag.AdminPage = "Users";
        ViewData["Title"] = "Admin — Users";

        var identityUsers = await _userManager.Users.AsNoTracking()
            .Where(u => u.Email != ArchiveUserEmail)
            .OrderBy(u => u.Email)
            .ToListAsync();

        var reservationCounts = await _db.Bookings.AsNoTracking()
            .GroupBy(b => b.UserId)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.UserId, x => x.Count);

        var favoriteCounts = await _db.Favorites.AsNoTracking()
            .GroupBy(f => f.UserId)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.UserId, x => x.Count);

        var notificationCounts = await _db.Notifications.AsNoTracking()
            .GroupBy(n => n.UserId)
            .Select(g => new { UserId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.UserId, x => x.Count);

        var rows = new List<AdminUserRowViewModel>(identityUsers.Count);
        var adminCount = 0;
        var personnelCount = 0;
        foreach (var u in identityUsers)
        {
            var roles = await _userManager.GetRolesAsync(u);
            var isAdmin = roles.Contains("Admin");
            var isPersonnel = roles.Contains("Personnel");
            if (isAdmin) adminCount++;
            if (isPersonnel) personnelCount++;
            rows.Add(new AdminUserRowViewModel
            {
                Id = u.Id,
                Email = u.Email ?? "",
                DisplayName = string.IsNullOrWhiteSpace(u.DisplayName) ? "—" : u.DisplayName,
                RolesSummary = roles.Count == 0 ? "—" : string.Join(", ", roles.OrderBy(r => r)),
                EmailConfirmed = u.EmailConfirmed,
                ReservationsCount = reservationCounts.TryGetValue(u.Id, out var rc) ? rc : 0,
                FavoritesCount = favoriteCounts.TryGetValue(u.Id, out var fc) ? fc : 0,
                IsAdmin = isAdmin,
                IsPersonnel = isPersonnel,
                HasRelatedRecords =
                    (reservationCounts.TryGetValue(u.Id, out var brc) && brc > 0) ||
                    (favoriteCounts.TryGetValue(u.Id, out var fvc) && fvc > 0) ||
                    (notificationCounts.TryGetValue(u.Id, out var nfc) && nfc > 0)
            });
        }

        ViewBag.TotalUsers = rows.Count;
        ViewBag.AdminUsers = adminCount;
        ViewBag.PersonnelUsers = personnelCount;
        ViewBag.TravelerUsers = rows.Count - rows.Count(r => r.IsAdmin || r.IsPersonnel);
        ViewBag.CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        return View(rows);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("users/make-admin/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MakeAdmin(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        if (!await _userManager.IsInRoleAsync(user, "Admin"))
        {
            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (!result.Succeeded)
            {
                TempData["UsersMessageType"] = "warning";
                TempData["UsersMessage"] = "Could not assign Admin role.";
            }
            else
            {
                TempData["UsersMessageType"] = "success";
                TempData["UsersMessage"] = "Admin role updated successfully.";
            }
        }

        return RedirectToAction(nameof(Users));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("users/make-personnel/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MakePersonnel(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        if (!await _userManager.IsInRoleAsync(user, "Personnel"))
        {
            var result = await _userManager.AddToRoleAsync(user, "Personnel");
            TempData["UsersMessageType"] = result.Succeeded ? "success" : "warning";
            TempData["UsersMessage"] = result.Succeeded
                ? "Personnel role updated successfully."
                : "Could not assign Personnel role.";
        }

        return RedirectToAction(nameof(Users));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("users/remove-personnel/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemovePersonnel(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        if (!await _userManager.IsInRoleAsync(user, "Personnel"))
            return RedirectToAction(nameof(Users));

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        if (user.Id == currentUserId)
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "You cannot remove Personnel role from your own account.";
            return RedirectToAction(nameof(Users));
        }

        var result = await _userManager.RemoveFromRoleAsync(user, "Personnel");
        TempData["UsersMessageType"] = result.Succeeded ? "success" : "warning";
        TempData["UsersMessage"] = result.Succeeded
            ? "Personnel role updated successfully."
            : "Could not remove Personnel role.";

        return RedirectToAction(nameof(Users));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("users/remove-admin/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveAdmin(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        if (!await _userManager.IsInRoleAsync(user, "Admin"))
            return RedirectToAction(nameof(Users));

        var adminIds = new List<string>();
        foreach (var u in _userManager.Users)
        {
            if (await _userManager.IsInRoleAsync(u, "Admin"))
                adminIds.Add(u.Id);
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        if (adminIds.Count <= 1 && user.Id == currentUserId)
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "You cannot remove Admin role from the last active admin account.";
            return RedirectToAction(nameof(Users));
        }

        var result = await _userManager.RemoveFromRoleAsync(user, "Admin");
        TempData["UsersMessageType"] = result.Succeeded ? "success" : "warning";
        TempData["UsersMessage"] = result.Succeeded
            ? "Admin role updated successfully."
            : "Could not remove Admin role.";

        return RedirectToAction(nameof(Users));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("users/delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
        if (string.Equals(user.Email, ArchiveUserEmail, StringComparison.OrdinalIgnoreCase))
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "System archive account is protected.";
            return RedirectToAction(nameof(Users));
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        if (user.Id == currentUserId)
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "You cannot delete the currently logged-in account.";
            return RedirectToAction(nameof(Users));
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
        if (isAdmin)
        {
            var adminCount = 0;
            foreach (var u in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(u, "Admin"))
                    adminCount++;
            }

            if (adminCount <= 1)
            {
                TempData["UsersMessageType"] = "warning";
                TempData["UsersMessage"] = "The last admin account is protected and cannot be deleted.";
                return RedirectToAction(nameof(Users));
            }
        }

        var archiveUser = await EnsureArchiveUserAsync();

        var bookings = await _db.Bookings.Where(b => b.UserId == user.Id).ToListAsync();
        foreach (var booking in bookings)
            booking.UserId = archiveUser.Id;

        var favorites = await _db.Favorites.Where(f => f.UserId == user.Id).ToListAsync();
        if (favorites.Count > 0)
            _db.Favorites.RemoveRange(favorites);

        var notifications = await _db.Notifications.Where(n => n.UserId == user.Id).ToListAsync();
        if (notifications.Count > 0)
            _db.Notifications.RemoveRange(notifications);

        if (bookings.Count > 0 || favorites.Count > 0 || notifications.Count > 0)
            await _db.SaveChangesAsync();

        var result = await _userManager.DeleteAsync(user);
        TempData["UsersMessageType"] = result.Succeeded ? "success" : "warning";
        TempData["UsersMessage"] = result.Succeeded
            ? "User account deleted successfully."
            : "Could not delete the user account.";

        return RedirectToAction(nameof(Users));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("analytics")]
    public async Task<IActionResult> Analytics()
    {
        ViewBag.AdminPage = "Analytics";
        ViewData["Title"] = "Admin — Analytics";

        var totalUsersTask = _db.Users.CountAsync();
        var totalBookingsTask = _db.Bookings.CountAsync();
        var pendingTask = _db.Bookings.CountAsync(b => b.Status == BookingStatus.Pending);
        var confirmedTask = _db.Bookings.CountAsync(b => b.Status == BookingStatus.Accepted);
        var destinationsTask = _db.Destinations.CountAsync();
        var experiencesTask = _db.Experiences.CountAsync();
        var toursTask = _db.Tours.CountAsync();
        var staysTask = _db.Stays.CountAsync();
        var favoritesTask = _db.Favorites.CountAsync();

        await Task.WhenAll(
            totalUsersTask, totalBookingsTask, pendingTask, confirmedTask,
            destinationsTask, experiencesTask, toursTask, staysTask, favoritesTask);

        var totalUsers = await totalUsersTask;
        var totalBookings = await totalBookingsTask;
        var pending = await pendingTask;
        var confirmed = await confirmedTask;
        var destinations = await destinationsTask;
        var experiences = await experiencesTask;
        var tours = await toursTask;
        var stays = await staysTask;
        var favorites = await favoritesTask;

        var recent = await _db.Bookings.AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .OrderByDescending(b => b.CreatedAtUtc)
            .Take(12)
            .ToListAsync();

        var recentRows = recent.Select(b => new AdminAnalyticsBookingRowViewModel
        {
            Id = b.Id,
            GuestEmail = b.User?.Email ?? b.UserId,
            ItemLabel = b.Tour?.Title ?? b.Stay?.Name ?? b.Experience?.Title ?? b.Destination?.Title ?? "—",
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            Status = b.Status,
            CreatedAtUtc = b.CreatedAtUtc
        }).ToList();

        var mostReserved = recent
            .GroupBy(b => b.Tour?.Title ?? b.Stay?.Name ?? b.Experience?.Title ?? b.Destination?.Title ?? "—")
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault() ?? "—";

        var mostFavorited = await _db.Favorites.AsNoTracking()
            .GroupBy(f => f.TargetKind)
            .Select(g => new { g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Select(x => x.Key.ToString())
            .FirstOrDefaultAsync() ?? "—";

        var latestUser = await _db.Users.AsNoTracking()
            .OrderByDescending(u => u.Id)
            .Select(u => u.Email)
            .FirstOrDefaultAsync() ?? "—";

        var vm = new AdminAnalyticsViewModel
        {
            TotalUsers = totalUsers,
            TotalBookings = totalBookings,
            PendingBookings = pending,
            ConfirmedBookings = confirmed,
            TotalDestinations = destinations,
            TotalExperiences = experiences,
            TotalTours = tours,
            TotalStays = stays,
            TotalFavorites = favorites,
            MostReservedItem = mostReserved,
            MostFavoritedItem = mostFavorited,
            LatestUserEmail = latestUser,
            RecentBookings = recentRows
        };

        return View(vm);
    }

    [HttpGet("settings")]
    [Authorize(Roles = "Admin")]
    public IActionResult Settings()
    {
        ViewBag.AdminPage = "Settings";
        ViewData["Title"] = "Admin — Settings";

        var provider = _db.Database.ProviderName ?? "SQLite";
        if (provider.Contains("Sqlite", StringComparison.OrdinalIgnoreCase))
            provider = "SQLite";

        var vm = new AdminSettingsViewModel
        {
            EnvironmentName = _env.EnvironmentName,
            DatabaseProvider = provider,
            CurrentYear = DateTime.UtcNow.Year
        };

        return View(vm);
    }

    [HttpGet("listings/new")]
    [Authorize(Roles = "Admin")]
    public IActionResult NewListing()
    {
        ViewBag.AdminPage = "Listings";
        ViewData["Title"] = "Admin — New listing";
        return View();
    }

    private async Task<ApplicationUser> EnsureArchiveUserAsync()
    {
        var archiveUser = await _userManager.FindByEmailAsync(ArchiveUserEmail);
        if (archiveUser != null) return archiveUser;

        archiveUser = new ApplicationUser
        {
            UserName = ArchiveUserEmail,
            Email = ArchiveUserEmail,
            EmailConfirmed = true,
            DisplayName = "Archived Traveler",
            LockoutEnabled = true,
            LockoutEnd = DateTimeOffset.MaxValue
        };

        var createResult = await _userManager.CreateAsync(archiveUser, $"Arc!{Guid.NewGuid():N}aA1");
        if (!createResult.Succeeded)
            throw new InvalidOperationException("Could not create archive user for booking history preservation.");

        return archiveUser;
    }
}
