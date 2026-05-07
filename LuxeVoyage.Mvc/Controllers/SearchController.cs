using LuxeVoyage.Mvc.Data;
using LuxeVoyage.Mvc.Mapping;
using LuxeVoyage.Mvc.Models;
using LuxeVoyage.Mvc.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Controllers;

/// <summary>Hero “Explore” search lands here via GET /search.</summary>
[Route("search")]
public class SearchController : Controller
{
    private readonly ApplicationDbContext _db;

    public SearchController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string? query, int? destinationId, string? date, string? dateEnd)
    {
        ViewBag.NavSection = "Home";
        ViewData["Title"] = "Search results | LuxeVoyage";

        var q = query?.Trim() ?? "";
        var hasText = !string.IsNullOrEmpty(q);
        var qLower = q.ToLowerInvariant();

        Destination? picked = null;
        if (destinationId is > 0)
            picked = await _db.Destinations.AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == destinationId.Value && d.IsActive);

        // -------- Destinations
        List<SearchHitVm> destinations;
        if (picked != null)
        {
            List<SearchHitVm> extra = new();
            if (hasText)
            {
                var extraRows = await _db.Destinations.AsNoTracking()
                    .Where(d => d.IsActive && d.IsVisibleOnListing && d.Id != picked.Id &&
                                (d.Title.ToLower().Contains(qLower) ||
                                 d.Slug.ToLower().Contains(qLower) ||
                                 (d.LocationLabel != null && d.LocationLabel.ToLower().Contains(qLower))))
                    .OrderBy(d => d.Title)
                    .Take(12)
                    .ToListAsync();
                extra = extraRows.Select(ToDestinationHit).ToList();
            }

            destinations = new List<SearchHitVm> { ToDestinationHit(picked) };
            destinations.AddRange(extra);
        }
        else if (hasText)
        {
            var destRows = await _db.Destinations.AsNoTracking()
                .Where(d => d.IsActive && d.IsVisibleOnListing &&
                    (d.Title.ToLower().Contains(qLower) ||
                    d.Slug.ToLower().Contains(qLower) ||
                    (d.LocationLabel != null && d.LocationLabel.ToLower().Contains(qLower)) ||
                    (d.Summary != null && d.Summary.ToLower().Contains(qLower)) ||
                    (d.CardTitle != null && d.CardTitle.ToLower().Contains(qLower)) ||
                    (d.CardSummary != null && d.CardSummary.ToLower().Contains(qLower))))
                .OrderBy(d => d.Title)
                .Take(24)
                .ToListAsync();
            destinations = destRows.Select(ToDestinationHit).ToList();
        }
        else
        {
            destinations = new List<SearchHitVm>();
        }

        // -------- Experiences
        IQueryable<Experience> expQ = _db.Experiences.AsNoTracking().Where(e => e.IsActive);
        if (picked != null)
        {
            expQ = expQ.Where(e => e.Region == picked.Region);
            if (hasText)
                expQ = expQ.Where(e =>
                    e.Title.ToLower().Contains(qLower) ||
                    (e.Summary != null && e.Summary.ToLower().Contains(qLower)));
        }
        else if (hasText)
        {
            expQ = expQ.Where(e =>
                e.Title.ToLower().Contains(qLower) ||
                e.Slug.ToLower().Contains(qLower) ||
                (e.LocationLabel != null && e.LocationLabel.ToLower().Contains(qLower)) ||
                (e.Summary != null && e.Summary.ToLower().Contains(qLower)));
        }
        else
        {
            expQ = expQ.Where(_ => false);
        }

        var expRows = await expQ.OrderBy(e => e.Title).Take(24).ToListAsync();
        var experiences = expRows.Select(e => new SearchHitVm
        {
            Title = e.Title,
            Subtitle = string.Join(" · ",
                new[] { e.LocationLabel, CatalogQueryHelper.RegionDisplay(e.Region) }.Where(s => !string.IsNullOrEmpty(s))),
            ImageUrl = string.IsNullOrWhiteSpace(e.CardImageUrl) ? e.ImageUrl : e.CardImageUrl.Trim(),
            DetailUrl = Url.Action(nameof(ExperiencesController.Detail), "Experiences", new { id = e.Slug }) ?? ""
        }).ToList();

        // -------- Tours
        List<SearchHitVm> tours;
        if (hasText)
        {
            var tourRows = await _db.Tours.AsNoTracking().Where(t => t.IsActive)
                .Where(t =>
                    t.Title.ToLower().Contains(qLower) ||
                    t.Slug.ToLower().Contains(qLower) ||
                    (t.Summary != null && t.Summary.ToLower().Contains(qLower)) ||
                    t.CategoryLabel.ToLower().Contains(qLower))
                .OrderBy(t => t.Title)
                .Take(24)
                .ToListAsync();
            tours = tourRows.Select(ToTourHit).ToList();
        }
        else if (picked != null)
        {
            var feat = await _db.Tours.AsNoTracking().Where(t => t.IsActive)
                .OrderByDescending(t => t.ReviewCount)
                .Take(8)
                .ToListAsync();
            tours = feat.Select(ToTourHit).ToList();
        }
        else
        {
            tours = new List<SearchHitVm>();
        }

        // -------- Stays
        IQueryable<Stay> stayQ = _db.Stays.AsNoTracking().Where(s => s.IsActive);
        if (picked != null)
        {
            stayQ = stayQ.Where(s => s.Region == picked.Region);
            if (hasText)
                stayQ = stayQ.Where(s =>
                    s.Name.ToLower().Contains(qLower) ||
                    s.CityLine.ToLower().Contains(qLower));
        }
        else if (hasText)
        {
            stayQ = stayQ.Where(s =>
                s.Name.ToLower().Contains(qLower) ||
                s.Slug.ToLower().Contains(qLower) ||
                s.CityLine.ToLower().Contains(qLower));
        }
        else
        {
            stayQ = stayQ.Where(_ => false);
        }

        var stayRows = await stayQ.OrderBy(s => s.Name).Take(24).ToListAsync();
        var stays = stayRows.Select(s => new SearchHitVm
        {
            Title = s.Name,
            Subtitle = $"{s.CityLine} · ${s.PricePerNight:0}/night · {s.StarRating}★",
            ImageUrl = s.ImageUrl,
            DetailUrl = Url.Action(nameof(HotelsController.Detail), "Hotels", new { id = s.Slug }) ?? ""
        }).ToList();

        string? summary = null;
        if (DateTime.TryParse(date, out var ds))
        {
            if (DateTime.TryParse(dateEnd, out var de) && de >= ds)
                summary = $"{ds:d} – {de:d}";
            else
                summary = $"{ds:d}";
        }

        var vm = new SearchResultsViewModel
        {
            Query = q,
            DestinationId = destinationId,
            DateParam = date,
            DateEndParam = dateEnd,
            TravelDatesSummary = summary,
            Destinations = destinations,
            Experiences = experiences,
            Tours = tours,
            Stays = stays
        };

        return View("Results", vm);
    }

    private SearchHitVm ToDestinationHit(Destination d) => new()
    {
        Title = DestinationDisplayMapper.EffectiveCardTitle(d),
        Subtitle = string.Join(" · ",
            new[] { d.LocationLabel, DestinationDisplayMapper.EffectiveCardRegionDisplay(d) }.Where(s => !string.IsNullOrEmpty(s))),
        ImageUrl = DestinationDisplayMapper.EffectiveCardImageUrl(d),
        DetailUrl = Url.Action(nameof(DestinationsController.Detail), "Destinations", new { id = d.Slug }) ?? ""
    };

    private SearchHitVm ToTourHit(Tour t) => new()
    {
        Title = t.Title,
        Subtitle = $"{t.CategoryLabel} · From ${t.Price:0}",
        ImageUrl = t.ImageUrl,
        DetailUrl = Url.Action(nameof(ToursController.Detail), "Tours", new { slug = t.Slug }) ?? ""
    };
}
