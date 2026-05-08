namespace LuxeVoyage.Mvc.Helpers;

public static class CartItemTypes
{
    public const string Experience = "Experience";
    public const string Tour = "Tour";
    public const string Stay = "Stay";
    public const string Destination = "Destination";

    public static bool TryNormalize(string? raw, out string canonical)
    {
        canonical = "";
        if (string.IsNullOrWhiteSpace(raw))
            return false;
        if (string.Equals(raw, Experience, StringComparison.OrdinalIgnoreCase))
        {
            canonical = Experience;
            return true;
        }

        if (string.Equals(raw, Tour, StringComparison.OrdinalIgnoreCase))
        {
            canonical = Tour;
            return true;
        }

        if (string.Equals(raw, Stay, StringComparison.OrdinalIgnoreCase))
        {
            canonical = Stay;
            return true;
        }

        if (string.Equals(raw, Destination, StringComparison.OrdinalIgnoreCase))
        {
            canonical = Destination;
            return true;
        }

        return false;
    }
}
