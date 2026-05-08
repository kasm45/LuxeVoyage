using LuxeVoyage.Mvc.Models;
using Microsoft.AspNetCore.Identity;

namespace LuxeVoyage.Mvc.Helpers;

/// <summary>Guest labels for admin reservations — prefers immutable booking snapshots over live Identity fields.</summary>
public static class AdminUserDisplayHelper
{
    public static bool IsArchivedIdentity(ApplicationUser? user)
    {
        if (user == null)
            return false;
        if (!string.IsNullOrEmpty(user.Email) &&
            user.Email.EndsWith("@archived.local", StringComparison.OrdinalIgnoreCase))
            return true;
        if (!string.IsNullOrEmpty(user.UserName) &&
            user.UserName.EndsWith("@archived.local", StringComparison.OrdinalIgnoreCase))
            return true;
        if (string.Equals(user.DisplayName?.Trim(), "Removed user", StringComparison.Ordinal))
            return true;
        return false;
    }

    private static bool IsArchivedEmail(string? email) =>
        !string.IsNullOrEmpty(email) &&
        email.EndsWith("@archived.local", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Display priority: snapshot name → snapshot email → live non-archived user → removed customer.
    /// </summary>
    public static string GuestDisplayName(Booking booking, ApplicationUser? user)
    {
        if (!string.IsNullOrWhiteSpace(booking.CustomerNameSnapshot))
            return booking.CustomerNameSnapshot.Trim();

        if (!string.IsNullOrWhiteSpace(booking.CustomerEmailSnapshot))
            return booking.CustomerEmailSnapshot.Trim();

        if (user != null && !IsArchivedIdentity(user))
        {
            if (!string.IsNullOrWhiteSpace(user.DisplayName))
                return user.DisplayName.Trim();
            if (!string.IsNullOrWhiteSpace(user.Email) && !IsArchivedEmail(user.Email))
                return user.Email.Trim();
        }

        return "Removed customer";
    }

    /// <summary>Secondary email column; omits archived synthetic addresses.</summary>
    public static string GuestEmailLabel(Booking booking, ApplicationUser? user)
    {
        if (!string.IsNullOrWhiteSpace(booking.CustomerEmailSnapshot) &&
            !IsArchivedEmail(booking.CustomerEmailSnapshot))
            return booking.CustomerEmailSnapshot.Trim();

        if (user != null && !IsArchivedIdentity(user) &&
            !string.IsNullOrWhiteSpace(user.Email) && !IsArchivedEmail(user.Email))
            return user.Email.Trim();

        return "";
    }

    /// <summary>Fallback when only Identity is available (no booking context).</summary>
    public static string GuestLabel(IdentityUser? user)
    {
        if (user == null)
            return "";
        if (user is ApplicationUser app && IsArchivedIdentity(app))
            return "Removed customer";
        return user.Email ?? user.Id;
    }
}
