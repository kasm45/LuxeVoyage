using System.Globalization;
using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Helpers;
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
    /// <summary>Cookie: hide Analytics "Recent reservations" rows with CreatedAtUtc ≤ this UTC instant (display-only).</summary>
    private const string AnalyticsRecentReservationsHiddenBeforeCookie = "AnalyticsRecentReservationsHiddenBeforeUtc";

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
    public async Task<IActionResult> Dashboard()
    {
        if (!User.IsInRole("Admin"))
            return RedirectToAction(nameof(AdminManageBookingsController.Index), "AdminManageBookings");

        var paid = _db.Payments.AsNoTracking().Where(p => p.Status == PaymentStatus.Paid);

        // SQLite provider cannot Sum decimal in SQL; materialize then aggregate in memory.
        var paidAmounts = await paid.Select(p => p.Amount).ToListAsync();
        var totalPaidRevenueUsd = paidAmounts.Sum();
        var paidBookingsCount = await paid.Select(p => p.BookingId).Distinct().CountAsync();

        var awaitingStaffReview = await _db.Bookings.CountAsync(b => b.Status == BookingStatus.Pending);

        var acceptedIds = await _db.Bookings.AsNoTracking()
            .Where(b => b.Status == BookingStatus.Accepted)
            .Select(b => b.Id)
            .ToListAsync();

        var paidBookingIdSet = await paid.Select(p => p.BookingId).Distinct().ToListAsync();
        var paidBkSet = paidBookingIdSet.ToHashSet();
        var awaitingDemoPayment = acceptedIds.Count(id => !paidBkSet.Contains(id));

        var pendingUnpaidRequestsCount = awaitingStaffReview + awaitingDemoPayment;

        var recentPayments = await paid
            .OrderByDescending(p => p.PaidAtUtc)
            .Take(8)
            .Include(p => p.User)
            .Include(p => p.Booking).ThenInclude(b => b.Tour)
            .Include(p => p.Booking).ThenInclude(b => b.Stay)
            .Include(p => p.Booking).ThenInclude(b => b.Experience)
            .Include(p => p.Booking).ThenInclude(b => b.Destination)
            .ToListAsync();

        var recentRows = recentPayments.Select(p => new AdminRecentPaymentRowVm
        {
            Amount = p.Amount,
            Currency = p.Currency,
            PaidAtUtc = p.PaidAtUtc,
            TransactionReference = p.TransactionReference,
            GuestEmail = AdminUserDisplayHelper.GuestDisplayName(p.Booking, p.User),
            ItemLabel = BookingItemLabel(p.Booking)
        }).ToList();

        var vm = new AdminDashboardMainViewModel
        {
            TotalPaidRevenueUsd = totalPaidRevenueUsd,
            PaidBookingsCount = paidBookingsCount,
            PendingUnpaidRequestsCount = pendingUnpaidRequestsCount,
            RecentPaidPayments = recentRows
        };

        ViewBag.AdminPage = "Dashboard";
        ViewData["Title"] = "LuxeVoyage Admin Dashboard";
        return View(vm);
    }

    private static string BookingItemLabel(Booking b)
    {
        return b.Tour?.Title ?? b.Stay?.Name ?? b.Experience?.Title ?? b.Destination?.Title ?? "Trip";
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("users")]
    public async Task<IActionResult> Users(bool showRemoved = false)
    {
        ViewBag.AdminPage = "Users";
        ViewData["Title"] = "Admin — Users";
        ViewBag.ShowRemovedAccounts = showRemoved;

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
        var activeAdminCount = 0;
        foreach (var u in identityUsers)
        {
            var roles = await _userManager.GetRolesAsync(u);
            var isArchived = AdminUserDisplayHelper.IsArchivedIdentity(u);
            var isAdmin = roles.Contains("Admin");
            var isPersonnel = roles.Contains("Personnel");
            var isLockedOut = await _userManager.IsLockedOutAsync(u);
            if (isAdmin && !isArchived)
                adminCount++;
            if (isPersonnel && !isArchived)
                personnelCount++;
            if (isAdmin && !isArchived && !isLockedOut)
                activeAdminCount++;
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
                    (notificationCounts.TryGetValue(u.Id, out var nfc) && nfc > 0),
                IsRemoved = isArchived,
                IsDisabled = isLockedOut && !isArchived,
                CanReactivate = isLockedOut && !isArchived
            });
        }

        ViewBag.ActiveUsers = rows.Count(r => !r.IsRemoved);
        ViewBag.RemovedAccounts = rows.Count(r => r.IsRemoved);
        ViewBag.AdminUsers = adminCount;
        ViewBag.PersonnelUsers = personnelCount;
        ViewBag.TravelerUsers = rows.Count(r => !r.IsAdmin && !r.IsPersonnel && !r.IsRemoved);
        ViewBag.ActiveAdminCount = activeAdminCount;
        ViewBag.CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        var displayRows = showRemoved ? rows : rows.Where(r => !r.IsRemoved).ToList();
        ViewBag.VisibleUsers = displayRows.Count;

        return View(displayRows);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("users/make-admin/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MakeAdmin(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
        if (AdminUserDisplayHelper.IsArchivedIdentity(user))
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "Removed accounts cannot be assigned roles.";
            return RedirectToAction(nameof(Users));
        }

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
        if (AdminUserDisplayHelper.IsArchivedIdentity(user))
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "Removed accounts cannot be assigned roles.";
            return RedirectToAction(nameof(Users));
        }

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
        if (AdminUserDisplayHelper.IsArchivedIdentity(user))
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "Removed accounts cannot be modified.";
            return RedirectToAction(nameof(Users));
        }

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
        if (AdminUserDisplayHelper.IsArchivedIdentity(user))
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "Removed accounts cannot be modified.";
            return RedirectToAction(nameof(Users));
        }

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
    [HttpPost("users/remove-account/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveUserAccount(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
        if (string.Equals(user.Email, ArchiveUserEmail, StringComparison.OrdinalIgnoreCase))
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "System archive account is protected.";
            return RedirectToAction(nameof(Users));
        }

        if (AdminUserDisplayHelper.IsArchivedIdentity(user))
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "This account has already been removed.";
            return RedirectToAction(nameof(Users));
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        if (user.Id == currentUserId)
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "You cannot remove the currently logged-in account.";
            return RedirectToAction(nameof(Users));
        }

        if (await _userManager.IsInRoleAsync(user, "Admin"))
        {
            var activeAdmins = 0;
            foreach (var u in _userManager.Users)
            {
                if (!await _userManager.IsInRoleAsync(u, "Admin"))
                    continue;
                if (AdminUserDisplayHelper.IsArchivedIdentity(u))
                    continue;
                if (await _userManager.IsLockedOutAsync(u))
                    continue;
                activeAdmins++;
            }

            if (activeAdmins <= 1)
            {
                TempData["UsersMessageType"] = "warning";
                TempData["UsersMessage"] = "The last active admin account cannot be removed.";
                return RedirectToAction(nameof(Users));
            }
        }

        var bookingsForUser = await _db.Bookings.Where(b => b.UserId == user.Id).ToListAsync();
        foreach (var b in bookingsForUser)
        {
            if (string.IsNullOrWhiteSpace(b.CustomerNameSnapshot))
            {
                var nm = string.IsNullOrWhiteSpace(user.DisplayName) ? null : user.DisplayName.Trim();
                if (!string.IsNullOrWhiteSpace(nm))
                    b.CustomerNameSnapshot = nm;
            }

            if (string.IsNullOrWhiteSpace(b.CustomerEmailSnapshot))
            {
                var em = string.IsNullOrWhiteSpace(user.Email) ? null : user.Email.Trim();
                if (!string.IsNullOrWhiteSpace(em))
                    b.CustomerEmailSnapshot = em;
            }
        }

        if (bookingsForUser.Count > 0)
            await _db.SaveChangesAsync();

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles.ToList())
        {
            var rr = await _userManager.RemoveFromRoleAsync(user, role);
            if (!rr.Succeeded)
            {
                TempData["UsersMessageType"] = "warning";
                TempData["UsersMessage"] = "Could not update roles before removing the account.";
                return RedirectToAction(nameof(Users));
            }
        }

        var archivedLogin = $"removed-{user.Id}@archived.local";
        user.DisplayName = "Removed user";
        user.EmailConfirmed = false;

        var userNameResult = await _userManager.SetUserNameAsync(user, archivedLogin);
        if (!userNameResult.Succeeded)
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "Could not anonymize login name.";
            return RedirectToAction(nameof(Users));
        }

        var emailResult = await _userManager.SetEmailAsync(user, archivedLogin);
        if (!emailResult.Succeeded)
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "Could not anonymize email.";
            return RedirectToAction(nameof(Users));
        }

        await _userManager.UpdateAsync(user);

        await _userManager.SetLockoutEnabledAsync(user, true);
        await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        await _userManager.UpdateSecurityStampAsync(user);

        TempData["UsersMessageType"] = "success";
        TempData["UsersMessage"] =
            "Login removed and profile anonymized. Reservations and payments remain for audit.";

        return RedirectToAction(nameof(Users));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("users/reactivate/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReactivateUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
        if (string.Equals(user.Email, ArchiveUserEmail, StringComparison.OrdinalIgnoreCase))
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "System archive account cannot be reactivated.";
            return RedirectToAction(nameof(Users));
        }

        if (AdminUserDisplayHelper.IsArchivedIdentity(user))
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "Removed accounts cannot be reactivated.";
            return RedirectToAction(nameof(Users));
        }

        if (!await _userManager.IsLockedOutAsync(user))
        {
            TempData["UsersMessageType"] = "warning";
            TempData["UsersMessage"] = "This account is not disabled.";
            return RedirectToAction(nameof(Users));
        }

        await _userManager.SetLockoutEndDateAsync(user, null);
        await _userManager.SetLockoutEnabledAsync(user, true);

        TempData["UsersMessageType"] = "success";
        TempData["UsersMessage"] = "Account reactivated. The user can sign in again.";

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

        DateTime? analyticsHiddenBeforeUtc = null;
        if (Request.Cookies.TryGetValue(AnalyticsRecentReservationsHiddenBeforeCookie, out var hiddenRaw))
        {
            if (!string.IsNullOrWhiteSpace(hiddenRaw) &&
                DateTimeOffset.TryParse(
                    hiddenRaw,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind,
                    out var hiddenOffset))
            {
                analyticsHiddenBeforeUtc = hiddenOffset.UtcDateTime;
            }
            else
            {
                Response.Cookies.Delete(AnalyticsRecentReservationsHiddenBeforeCookie, new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Secure = Request.IsHttps,
                    Path = "/admin"
                });
            }
        }

        // Insight card: unchanged when "clear view" cookie is set — still based on latest 12 bookings by date.
        var recentForInsight = await _db.Bookings.AsNoTracking()
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .OrderByDescending(b => b.CreatedAtUtc)
            .Take(12)
            .ToListAsync();

        var recentQuery = _db.Bookings.AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .AsQueryable();

        if (analyticsHiddenBeforeUtc.HasValue)
            recentQuery = recentQuery.Where(b => b.CreatedAtUtc > analyticsHiddenBeforeUtc.Value);

        var recent = await recentQuery
            .OrderByDescending(b => b.CreatedAtUtc)
            .Take(12)
            .ToListAsync();

        var recentRows = recent.Select(b => new AdminAnalyticsBookingRowViewModel
        {
            Id = b.Id,
            GuestEmail = AdminUserDisplayHelper.GuestDisplayName(b, b.User),
            ItemLabel = b.Tour?.Title ?? b.Stay?.Name ?? b.Experience?.Title ?? b.Destination?.Title ?? "—",
            StartDate = b.StartDate,
            EndDate = b.EndDate,
            Status = b.Status,
            CreatedAtUtc = b.CreatedAtUtc
        }).ToList();

        var mostReserved = recentForInsight
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

        ViewBag.AnalyticsRecentViewFiltered = analyticsHiddenBeforeUtc.HasValue;

        return View(vm);
    }

    /// <summary>Sets a browser cookie so Analytics "Recent reservations" only shows bookings created after this UTC time. Does not modify data.</summary>
    [HttpPost("analytics/clear-view")]
    [Authorize(Roles = "Admin")]
    [ValidateAntiForgeryToken]
    public IActionResult ClearAnalyticsView()
    {
        var utcNow = DateTimeOffset.UtcNow;
        Response.Cookies.Append(
            AnalyticsRecentReservationsHiddenBeforeCookie,
            utcNow.ToString("o", CultureInfo.InvariantCulture),
            new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = Request.IsHttps,
                IsEssential = true,
                Path = "/admin",
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            });

        TempData["AnalyticsMessage"] = "Analytics view cleared. Reservation history was not changed.";
        TempData["AnalyticsMessageType"] = "success";
        return RedirectToAction(nameof(Analytics));
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

}