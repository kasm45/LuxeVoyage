namespace LuxeVoyage.Mvc.Models;

/// <summary>Query-string values: art, nature, history, culinary</summary>
public enum ExperienceCategoryKind
{
    ArtCulture,
    NatureEscapes,
    HistoricalSites,
    CulinaryJourneys
}

/// <summary>Query-string values: europe, asia, americas, africa, middleeast</summary>
public enum RegionKind
{
    Europe,
    Asia,
    Americas,
    Africa,
    MiddleEast
}

public enum BookingStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed
}

public enum FavoriteTargetKind
{
    Experience = 0,
    Destination = 1,
    Stay = 2,
    Tour = 3
}
