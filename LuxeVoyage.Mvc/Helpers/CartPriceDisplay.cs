namespace LuxeVoyage.Mvc.Helpers;

/// <summary>Customer-facing price lines for basket cards (catalog strings are inconsistent; never throw).</summary>
public static class CartPriceDisplay
{
    public static string SavedLine(string? catalogHint)
    {
        if (string.IsNullOrWhiteSpace(catalogHint))
            return "Custom quote";

        return catalogHint.Trim();
    }

    /// <summary>Pending request row — concierge has not confirmed pricing yet.</summary>
    public static string PendingLine(string? catalogHint)
    {
        if (string.IsNullOrWhiteSpace(catalogHint))
            return "Custom quote";

        return $"Estimated · {catalogHint.Trim()}";
    }
}
