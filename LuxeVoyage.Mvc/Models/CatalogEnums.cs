using System.ComponentModel.DataAnnotations;

namespace LuxeVoyage.Mvc.Models;

/// <summary>Query-string values: art, nature, history, culinary</summary>
public enum ExperienceCategoryKind
{
    ArtCulture,
    NatureEscapes,
    HistoricalSites,
    CulinaryJourneys
}

/// <summary>Query-string values: europe, asia, middleeast, africa, north-america, south-america, oceania, pacific-islands</summary>
public enum RegionKind
{
    [Display(Name = "Europe")]
    Europe = 0,
    [Display(Name = "Asia")]
    Asia = 1,
    [Display(Name = "North America")]
    NorthAmerica = 2,
    [Display(Name = "North America")]
    Americas = 2,
    [Display(Name = "Africa")]
    Africa = 3,
    [Display(Name = "Middle East")]
    MiddleEast = 4,
    [Display(Name = "South America")]
    SouthAmerica = 5,
    [Display(Name = "Oceania")]
    Oceania = 6,
    [Display(Name = "Pacific Islands")]
    PacificIslands = 7
}

/// <summary>Stored as int in SQLite. Do not renumber without a data migration — values 0–3 are used in existing rows.</summary>
public enum BookingStatus
{
    Pending = 0,
    /// <summary>Admin accepted the request (displayed as &quot;Confirmed&quot; in several UIs).</summary>
    Accepted = 1,
    /// <summary>Admin rejected the request (displayed as &quot;Unavailable&quot; / cancelled styling in UIs).</summary>
    Rejected = 2,
    Completed = 3
}

public enum FavoriteTargetKind
{
    Experience = 0,
    Destination = 1,
    Stay = 2,
    Tour = 3
}
