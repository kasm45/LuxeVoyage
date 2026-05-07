using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxeVoyage.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReactivateAmalfiZermattFeaturedDestinations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE Destinations SET IsActive = 1, IsVisibleOnListing = 1
                WHERE Slug IN ('amalfi-coast', 'zermatt');
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE Destinations SET IsActive = 0
                WHERE Slug IN ('amalfi-coast', 'zermatt');
                """);
        }
    }
}
