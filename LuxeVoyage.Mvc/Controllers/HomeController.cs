using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Mapping;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

public class HomeController : Controller
{
    private static readonly string[] HomeFeaturedSlugPriority =
    [
        "amalfi-coast",
        "tokyo",
        "zermatt"
    ];

    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    [Route("")]
    [Route("Home")]
    [Route("Home/Index")]
    public async Task<IActionResult> Index()
    {
        ViewBag.NavSection = "Home";
        ViewData["Title"] = "LuxeVoyage";

        var destRows = await _db.Destinations.AsNoTracking()
            .Where(d => d.IsActive && d.IsVisibleOnListing)
            .OrderBy(d => d.Title)
            .ToListAsync();
        var suggestions = destRows.Select(d =>
        {
            var (city, country) = CityCountryLabels(d);
            var region = CatalogQueryHelper.RegionDisplay(d.Region);
            var listTitle = DestinationDisplayMapper.EffectiveCardTitle(d);
            var parts = new[]
            {
                listTitle,
                d.LocationLabel,
                d.BreadcrumbCity,
                d.BreadcrumbCurrent,
                region,
                city,
                country
            }.Where(s => !string.IsNullOrWhiteSpace(s));
            var searchText = string.Join(' ', parts);
            return new DestinationSuggestionClient
            {
                Id = d.Id,
                Title = listTitle,
                LocationLabel = d.LocationLabel,
                RegionLabel = region,
                City = city,
                Country = country,
                SearchText = searchText
            };
        }).ToList();

        var priority = HomeFeaturedSlugPriority.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var featuredRows = HomeFeaturedSlugPriority
            .Select(slug => destRows.FirstOrDefault(d =>
                string.Equals(d.Slug, slug, StringComparison.OrdinalIgnoreCase)))
            .Where(d => d != null)
            .Cast<Destination>()
            .Concat(destRows.Where(d => !priority.Contains(d.Slug)).OrderBy(d => d.Title))
            .Take(3)
            .ToList();

        var featuredCards = featuredRows
            .Select((d, i) => MapHomeFeaturedCard(d, i, featuredRows.Count))
            .ToList();

        var vm = new HomeIndexViewModel
        {
            DestinationSuggestionsJson = JsonSerializer.Serialize(suggestions, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }),
            FeaturedDestinations = featuredCards
        };

        return View(vm);
    }

    private static HomeFeaturedDestinationCard MapHomeFeaturedCard(Destination d, int index, int total)
    {
        var badges = new List<string>();
        var cb = d.CardBadge?.Trim();
        if (!string.IsNullOrEmpty(cb))
        {
            badges.AddRange(cb.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(b => b.ToUpperInvariant()));
        }

        if (badges.Count == 0)
            badges.Add(CatalogQueryHelper.CategoryDisplay(d.Category).ToUpperInvariant());

        badges = badges.Where(b => b.Length > 0).Take(2).ToList();

        var img = DestinationDisplayMapper.EffectiveCardImageUrl(d) ?? d.ImageUrl ?? "";

        return new HomeFeaturedDestinationCard
        {
            Slug = d.Slug,
            Title = DestinationDisplayMapper.EffectiveCardTitle(d),
            Summary = DestinationDisplayMapper.EffectiveCardSummary(d) ?? "",
            ImageUrl = img,
            Badges = badges,
            IsHeroTile = index == 0 && total > 0
        };
    }

    /// <summary>Derives searchable city/country strings from <see cref="Destination"/> breadcrumb and location fields.</summary>
    private static (string City, string Country) CityCountryLabels(Destination d)
    {
        var loc = d.LocationLabel?.Trim() ?? "";
        var city = (d.BreadcrumbCity ?? "").Trim();
        var country = "";

        if (loc.Contains(','))
        {
            var parts = loc.Split(',', 2, StringSplitOptions.TrimEntries);
            if (parts.Length >= 1 && string.IsNullOrEmpty(city))
                city = parts[0];
            if (parts.Length >= 2)
                country = parts[1];
            else if (parts.Length == 1 && loc.IndexOf(',') >= 0)
                country = "";
        }
        else if (!string.IsNullOrEmpty(loc))
            country = loc;

        return (city, country);
    }

    [Route("Home/Error")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
