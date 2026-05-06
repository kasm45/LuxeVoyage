using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController : Controller
{
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
        ViewBag.AdminPage = "Dashboard";
        ViewData["Title"] = "LuxeVoyage Admin Dashboard";
        return View();
    }

    [HttpGet("users")]
    public async Task<IActionResult> Users()
    {
        ViewBag.AdminPage = "Users";
        ViewData["Title"] = "Admin — Users";

        var identityUsers = await _userManager.Users.AsNoTracking()
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

        var rows = new List<AdminUserRowViewModel>(identityUsers.Count);
        foreach (var u in identityUsers)
        {
            var roles = await _userManager.GetRolesAsync(u);
            rows.Add(new AdminUserRowViewModel
            {
                Id = u.Id,
                Email = u.Email ?? "",
                DisplayName = string.IsNullOrWhiteSpace(u.DisplayName) ? "—" : u.DisplayName,
                RolesSummary = roles.Count == 0 ? "—" : string.Join(", ", roles.OrderBy(r => r)),
                EmailConfirmed = u.EmailConfirmed,
                ReservationsCount = reservationCounts.TryGetValue(u.Id, out var rc) ? rc : 0,
                FavoritesCount = favoriteCounts.TryGetValue(u.Id, out var fc) ? fc : 0
            });
        }

        return View(rows);
    }

    [HttpGet("analytics")]
    public async Task<IActionResult> Analytics()
    {
        ViewBag.AdminPage = "Analytics";
        ViewData["Title"] = "Admin — Analytics";

        var totalUsersTask = _db.Users.CountAsync();
        var totalBookingsTask = _db.Bookings.CountAsync();
        var pendingTask = _db.Bookings.CountAsync(b => b.Status == BookingStatus.Pending);
        var confirmedTask = _db.Bookings.CountAsync(b => b.Status == BookingStatus.Confirmed);
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
    public IActionResult NewListing()
    {
        ViewBag.AdminPage = "Listings";
        ViewData["Title"] = "Admin — New listing";
        return View();
    }
}
