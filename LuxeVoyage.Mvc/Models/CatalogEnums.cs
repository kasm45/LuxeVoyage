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

public enum BookingStatus
{
    Pending = 0,
    Accepted = 1,
    Confirmed = 1,
    Rejected = 2,
    Cancelled = 2,
    Completed = 3
}

public enum FavoriteTargetKind
{
    Experience = 0,
    Destination = 1,
    Stay = 2,
    Tour = 3
}
