using System.Security.Claims;
using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Helpers;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

public class AccountController : Controller
{
    private const string NoAdminAccessMessage = "This account does not have admin or personnel access.";
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext db)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _db = db;
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("account/login")]
    public IActionResult Login(string? returnUrl = null, string? mode = null)
    {
        ViewBag.NavSection = "Login";
        ViewData["Title"] = "LuxeVoyage - Login";
        var adminTab = string.Equals(mode, "admin", StringComparison.OrdinalIgnoreCase);
        return View(new LoginViewModel { ReturnUrl = returnUrl, AdminLogin = adminTab });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [Route("account/login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        ViewBag.NavSection = "Login";
        ViewData["Title"] = "LuxeVoyage - Login";

        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            if (await _userManager.IsLockedOutAsync(user))
            {
                ModelState.AddModelError(string.Empty,
                    "This account has been disabled. Please contact support.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (model.AdminLogin)
                {
                    var canStaffArea = await _userManager.IsInRoleAsync(user, "Admin") ||
                                       await _userManager.IsInRoleAsync(user, "Personnel");
                    if (!canStaffArea)
                    {
                        await _signInManager.SignOutAsync();
                        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                        ModelState.AddModelError(string.Empty, NoAdminAccessMessage);
                        return View(model);
                    }

                    TempData["Message"] = $"Welcome back{(string.IsNullOrEmpty(user.DisplayName) ? "" : ", " + user.DisplayName)}.";
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                        return RedirectToAction(nameof(AdminController.Dashboard), "Admin");
                    return RedirectToAction(nameof(AdminManageBookingsController.Index), "AdminManageBookings");
                }

                TempData["Message"] = $"Welcome back{(string.IsNullOrEmpty(user.DisplayName) ? "" : ", " + user.DisplayName)}.";
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        ModelState.AddModelError(string.Empty, "Invalid email or password.");
        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("account/register")]
    public IActionResult Register()
    {
        ViewBag.NavSection = "Login";
        ViewData["Title"] = "Create account - LuxeVoyage";
        return View(new RegisterViewModel());
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    [Route("account/register")]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        ViewBag.NavSection = "Login";
        ViewData["Title"] = "Create account - LuxeVoyage";

        if (!ModelState.IsValid)
            return View(model);

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            DisplayName = model.DisplayName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var err in result.Errors)
                ModelState.AddModelError(string.Empty, err.Description);
            return View(model);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        TempData["Message"] = "Your account was created. Welcome to LuxeVoyage.";
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("account/logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("account/access-denied")]
    public IActionResult AccessDenied()
    {
        ViewData["Title"] = "Access denied";
        return View();
    }

    [HttpGet]
    [Authorize]
    [Route("account/reservations")]
    public async Task<IActionResult> Reservations()
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        var bookings = await _db.Bookings.AsNoTracking()
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .Where(b => b.UserId == uid)
            .OrderByDescending(b => b.CreatedAtUtc)
            .ToListAsync();

        var ids = bookings.Select(b => b.Id).ToList();
        var paymentRows = await _db.Payments.AsNoTracking()
            .Where(p => ids.Contains(p.BookingId))
            .ToListAsync();

        var paidByBookingId = paymentRows
            .Where(p => p.Status == PaymentStatus.Paid)
            .GroupBy(p => p.BookingId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.PaidAtUtc).First());

        var failedBookingIds = paymentRows
            .Where(p => p.Status == PaymentStatus.Failed)
            .Select(p => p.BookingId)
            .Distinct()
            .ToHashSet();

        var today = DateTime.UtcNow.Date;

        var awaiting = new List<ReservationTripRowVm>();
        var paymentDue = new List<ReservationTripRowVm>();
        var upcomingPaid = new List<ReservationTripRowVm>();
        var pastTrips = new List<ReservationTripRowVm>();

        foreach (var b in bookings)
        {
            paidByBookingId.TryGetValue(b.Id, out var paidPmt);
            var hasFailed = paidPmt == null && failedBookingIds.Contains(b.Id);
            var checkout = Url.Action("Checkout", "Payments", new { bookingId = b.Id }) ?? "#";
            var confirm = Url.Action("Confirmation", "Bookings", new { id = b.Id }) ?? "#";

            var quote = BookingPaymentCalculator.TryGetPayableAmount(b);

            var row = new ReservationTripRowVm
            {
                Booking = b,
                PaidPayment = paidPmt,
                HasFailedPaymentAttempt = hasFailed,
                CheckoutUrl = checkout,
                ConfirmationUrl = confirm,
                CanPayDemo = quote.CanPay && quote.Amount > 0,
                AmountDueUsd = quote.CanPay ? quote.Amount : null
            };

            if (b.Status == BookingStatus.Pending)
                awaiting.Add(row);
            else if (b.Status == BookingStatus.Accepted && paidPmt == null)
                paymentDue.Add(row);
            else if (b.Status == BookingStatus.Accepted && paidPmt != null)
            {
                if (b.EndDate.Date >= today)
                    upcomingPaid.Add(row);
                else
                    pastTrips.Add(row);
            }
            else
                pastTrips.Add(row);
        }

        var page = new AccountReservationsPageVm
        {
            AwaitingReview = awaiting,
            PaymentDue = paymentDue,
            UpcomingPaidTrips = upcomingPaid,
            PastTrips = pastTrips
        };

        ViewBag.NavSection = "Account";
        ViewData["Title"] = "My reservations | LuxeVoyage";
        return View(page);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("account/reservations/{id:int}/cancel")]
    public async Task<IActionResult> CancelReservation(int id)
    {
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(uid))
            return Challenge();

        var booking = await _db.Bookings
            .Include(b => b.Tour)
            .Include(b => b.Stay)
            .Include(b => b.Experience)
            .Include(b => b.Destination)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (booking == null)
        {
            TempData["Error"] = "Reservation could not be found.";
            return RedirectToAction(nameof(Reservations));
        }

        if (booking.UserId != uid)
            return Forbid();

        if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Accepted)
        {
            TempData["Error"] = "This reservation cannot be cancelled.";
            return RedirectToAction(nameof(Reservations));
        }

        booking.Status = BookingStatus.Cancelled;
        booking.CancelledAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        var itemName = booking.Tour?.Title
            ?? booking.Stay?.Name
            ?? booking.Experience?.Title
            ?? booking.Destination?.Title
            ?? "Reservation";

        var admins = await _userManager.GetUsersInRoleAsync("Admin");
        foreach (var admin in admins)
        {
            _db.Notifications.Add(new Notification
            {
                UserId = admin.Id,
                Title = "Guest cancelled a reservation",
                Message = $"A traveler cancelled reservation #{booking.Id} ({itemName}).",
                Type = "ReservationCancelledByGuest",
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                ReservationId = booking.Id
            });
        }

        await _db.SaveChangesAsync();

        TempData["Message"] = "Your reservation was cancelled.";
        return RedirectToAction(nameof(Reservations));
    }
}
