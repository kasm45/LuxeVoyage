using LuxeVoyage.Mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace LuxeVoyage.Mvc.Data;

/// <summary>Ensures experience slugs match public URLs and inserts homepage/catalog gaps without duplicating rows.</summary>
public static class ExperienceCatalogRepair
{
    /// <summary>Experiences keyed by slug — inserted only if no row with that slug exists.</summary>
    private static readonly Experience[] EnsureBySlug =
    [
        new Experience
        {
            Title = "Private Vineyard Tour",
            Slug = "private-vineyard",
            Category = ExperienceCategoryKind.CulinaryJourneys,
            Region = RegionKind.Europe,
            ImageUrl =
                "https://lh3.googleusercontent.com/aida-public/AB6AXuD5Es1adMGCAvCmj1RVpGBoPpoP4UduxQOJ6wtmLgnD8wTPVSfOS_TGUxYHFst3LSrTS0CCNt1TTMCCFc3F3RY9Zs783o_MGJyg7qRa6xixyBrr6zR8nEG7nEhYY3BbRAu8xLfrOYUIuUk0e0oVBBksUL1hdmBRr4n3R-24W6HnRS4PqHU8QypZMn4WdvDVoXqnHKjzU3956vX0JOL8AG3ij24tVPlXqmfHahcpQUuh2Q0d4yTehJ8zysZnLLQ3xUGvw9RWD_q-Kao",
            Summary = "Exclusive tastings in hidden cellars across Tuscany.",
            LocationLabel = "Tuscany, Italy",
            Rating = 4.9,
            PriceHint = "$450",
            BreadcrumbRegion = "Europe",
            BreadcrumbCity = "Tuscany",
            BreadcrumbCurrent = "Chianti",
            TagLine = "Culinary"
        },
        new Experience
        {
            Title = "Aerial Canyon Tour",
            Slug = "aerial-canyon",
            Category = ExperienceCategoryKind.NatureEscapes,
            Region = RegionKind.NorthAmerica,
            ImageUrl =
                "https://lh3.googleusercontent.com/aida-public/AB6AXuBfLmPPvRhBGcmEobeHhHO2a_7h3knpyF4eiuW5aZdkCyYcsB0Z-qb17FEXLnEeYuUqBeH0vl690tP127JT9xggNqhPlFLMHU0ClCvTtSBIkduweCdWpBHiq63KiXPP2ul1psaTeQQR-U4alttTY_BS2h_ek1d_EGIzLc0awsNarTDMvXgqcMcgogxod07JTOuubpA3042YuMsHetKdqjEy8HYCw1lwhS_SdnXrYrPwaYFQpjcjbtYFjy5TWRpp4kRh3AVKgUObTRY",
            Summary = "Soar above majestic landscapes in a luxury chopper.",
            LocationLabel = "Southwest USA",
            Rating = 4.85,
            PriceHint = "$890",
            BreadcrumbRegion = "North America",
            BreadcrumbCity = "Grand Canyon",
            BreadcrumbCurrent = "Aerial",
            TagLine = "Adventure",
            IsActive = false
        }
    ];

    /// <summary>If title exists but slug should be canonical, update when slug is free.</summary>
    private static readonly (string Title, string Slug)[] CanonicalSlugByTitle =
    [
        ("The Alpine Sanctuary Peak", "alpine-sanctuary"),
        ("The Parisian Gallery Tour", "parisian-gallery"),
        ("Dubai Dune Safari", "dubai-dune-safari"),
        ("Fushimi Inari Twilight", "fushimi-inari-twilight"),
        ("Holistic Wellness Retreat", "holistic-wellness"),
        ("MoMA After Hours", "nyc-moma-after")
    ];

    public static async Task ApplyAsync(ApplicationDbContext ctx)
    {
        foreach (var template in EnsureBySlug)
        {
            if (await ctx.Experiences.AsNoTracking().AnyAsync(e => e.Slug == template.Slug))
                continue;

            var byTitle =
                await ctx.Experiences.FirstOrDefaultAsync(e => e.Title == template.Title);
            if (byTitle != null)
            {
                var slugFree =
                    !await ctx.Experiences.AnyAsync(e =>
                        e.Slug == template.Slug && e.Id != byTitle.Id);
                if (slugFree)
                    byTitle.Slug = template.Slug;
                continue;
            }

            ctx.Experiences.Add(Clone(template));
        }

        await ctx.SaveChangesAsync();

        foreach (var (title, slug) in CanonicalSlugByTitle)
        {
            var row = await ctx.Experiences.FirstOrDefaultAsync(e => e.Title == title);
            if (row == null || row.Slug == slug)
                continue;
            var taken = await ctx.Experiences.AnyAsync(e => e.Slug == slug && e.Id != row.Id);
            if (!taken)
                row.Slug = slug;
        }

        foreach (var e in await ctx.Experiences.Where(x => string.IsNullOrWhiteSpace(x.Slug)).ToListAsync())
        {
            var baseSlug = SlugUtil.FromTitle(e.Title);
            var slug = baseSlug;
            var n = 0;
            while (await ctx.Experiences.AnyAsync(x => x.Slug == slug && x.Id != e.Id))
                slug = baseSlug + "-" + ++n;
            e.Slug = slug;
        }

        foreach (var slug in new[] { "holistic-wellness" })
        {
            var row = await ctx.Experiences.FirstOrDefaultAsync(e => e.Slug == slug);
            if (row != null)
            {
                row.IsActive = true;
                row.IsVisibleOnListing = true;
            }
        }

        await ctx.SaveChangesAsync();
    }

    private static Experience Clone(Experience t) => new()
    {
        Title = t.Title,
        Slug = t.Slug,
        Category = t.Category,
        Region = t.Region,
        ImageUrl = t.ImageUrl,
        Summary = t.Summary,
        LocationLabel = t.LocationLabel,
        Rating = t.Rating,
        PriceHint = t.PriceHint,
        BreadcrumbRegion = t.BreadcrumbRegion,
        BreadcrumbCity = t.BreadcrumbCity,
        BreadcrumbCurrent = t.BreadcrumbCurrent,
        TagLine = t.TagLine
    };
}

internal static class SlugUtil
{
    public static string FromTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return "experience";
        var s = title.Trim().ToLowerInvariant();
        var chars = s.Select(c => char.IsLetterOrDigit(c) ? c : '-').ToArray();
        var joined = new string(chars);
        while (joined.Contains("--"))
            joined = joined.Replace("--", "-");
        return joined.Trim('-').Length > 0 ? joined.Trim('-') : "experience";
    }
}
