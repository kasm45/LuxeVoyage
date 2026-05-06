using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LuxeVoyage.Mvc.Controllers;

public class AccountController : Controller
{
    private const string NoAdminAccessMessage = "This account does not have admin access.";
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
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
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (model.AdminLogin)
                {
                    if (!await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        await _signInManager.SignOutAsync();
                        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                        ModelState.AddModelError(string.Empty, NoAdminAccessMessage);
                        return View(model);
                    }

                    TempData["Message"] = $"Welcome back{(string.IsNullOrEmpty(user.DisplayName) ? "" : ", " + user.DisplayName)}.";
                    return RedirectToAction(nameof(AdminController.Dashboard), "Admin");
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
}
