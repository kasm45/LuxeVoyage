using Microsoft.AspNetCore.Mvc;

namespace LuxeVoyage.Mvc.Controllers;

public class PagesController : Controller
{
    [Route("about")]
    [Route("journal")]
    public IActionResult About()
    {
        ViewBag.NavSection = "About";
        ViewData["Title"] = "About Us - LuxeVoyage";
        return View();
    }

    [Route("contact")]
    [HttpGet]
    public IActionResult Contact()
    {
        ViewBag.NavSection = "Contact";
        ViewData["Title"] = "Contact Us - LuxeVoyage";
        return View();
    }

    [Route("contact")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Contact(IFormCollection _)
    {
        TempData["ContactMessage"] = "Thank you for your message. Our concierge team will respond shortly.";
        return RedirectToAction(nameof(Contact));
    }

    [Route("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        return RedirectToAction("Login", "Account", new { returnUrl });
    }

    [Route("register")]
    public IActionResult Register()
    {
        return RedirectToAction("Register", "Account");
    }

    [Route("privacy")]
    public IActionResult Privacy()
    {
        ViewData["Title"] = "Privacy Policy - LuxeVoyage";
        return View();
    }

    [Route("terms")]
    public IActionResult Terms()
    {
        ViewData["Title"] = "Terms of Service - LuxeVoyage";
        return View();
    }

    [Route("sustainability")]
    public IActionResult Sustainability()
    {
        ViewData["Title"] = "Sustainability - LuxeVoyage";
        return View();
    }

    [Route("press-kit")]
    public IActionResult PressKit()
    {
        ViewData["Title"] = "Press Kit - LuxeVoyage";
        return View();
    }
}
