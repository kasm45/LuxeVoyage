namespace LuxeVoyage.Mvc.Models.ViewModels;

/// <summary>
/// Homepage hero; <see cref="DestinationSuggestionsJson"/> is consumed by home-search.js for the “Where to?” dropdown.
/// </summary>
public class HomeIndexViewModel
{
    /// <summary>JSON array of <see cref="DestinationSuggestionClient"/>.</summary>
    public string DestinationSuggestionsJson { get; set; } = "[]";

    /// <summary>Active destinations backing the Iconic Destinations bento grid (URLs use each row’s Slug).</summary>
    public IReadOnlyList<HomeFeaturedDestinationCard> FeaturedDestinations { get; set; } =
        Array.Empty<HomeFeaturedDestinationCard>();
}

/// <summary>Single tile on the homepage destinations grid.</summary>
public sealed class HomeFeaturedDestinationCard
{
    public string Slug { get; set; } = "";
    public string Title { get; set; } = "";
    public string Summary { get; set; } = "";
    public string ImageUrl { get; set; } = "";
    public IReadOnlyList<string> Badges { get; set; } = Array.Empty<string>();
    /// <summary>First tile spans the large bento cell when three tiles are shown.</summary>
    public bool IsHeroTile { get; set; }
}

/// <summary>Shape serialized into the homepage for client-side filtering.</summary>
public class DestinationSuggestionClient
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? LocationLabel { get; set; }
    public string RegionLabel { get; set; } = "";
    /// <summary>City or area name for client-side search (e.g. Istanbul).</summary>
    public string City { get; set; } = "";
    /// <summary>Country for client-side search (e.g. Turkey).</summary>
    public string Country { get; set; } = "";

    /// <summary>Normalized phrase for typeahead matching (title, location, region, city, country, breadcrumbs).</summary>
    public string SearchText { get; set; } = "";
}
