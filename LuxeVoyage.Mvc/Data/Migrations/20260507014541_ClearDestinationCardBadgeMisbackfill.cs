using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxeVoyage.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClearDestinationCardBadgeMisbackfill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First migration version copied TagLine into CardBadge, which would override the category chip on /destinations.
            migrationBuilder.Sql(
                """
                UPDATE Destinations
                SET CardBadge = NULL
                WHERE CardBadge IS NOT NULL
                  AND TagLine IS NOT NULL
                  AND trim(CardBadge) = trim(TagLine);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
