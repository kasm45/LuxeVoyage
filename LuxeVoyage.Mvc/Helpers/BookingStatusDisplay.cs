using LuxeVoyage.Mvc.Models;

namespace LuxeVoyage.Mvc.Helpers;

public static class BookingStatusDisplay
{
    /// <summary>Labels for traveler-facing and premium catalog UI.</summary>
    public static string TravelerLabel(BookingStatus status) => status switch
    {
        BookingStatus.Pending => "Pending",
        BookingStatus.Accepted => "Confirmed",
        BookingStatus.Rejected => "Unavailable",
        BookingStatus.Completed => "Completed",
        BookingStatus.Cancelled => "Cancelled",
        _ => status.ToString()
    };

    /// <summary>Labels aligned with admin reservation lists (Confirmed / Unavailable wording).</summary>
    public static string StaffHistoryLabel(BookingStatus status) => status switch
    {
        BookingStatus.Pending => "Pending",
        BookingStatus.Accepted => "Confirmed",
        BookingStatus.Rejected => "Unavailable",
        BookingStatus.Completed => "Completed",
        BookingStatus.Cancelled => "Cancelled",
        _ => status.ToString()
    };
}
