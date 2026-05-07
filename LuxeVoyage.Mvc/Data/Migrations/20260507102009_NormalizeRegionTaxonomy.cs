using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxeVoyage.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeRegionTaxonomy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE Destinations SET Region = 5, BreadcrumbRegion = 'South America'
                WHERE Slug IN ('patagonia-wild', 'rio-de-janeiro-brazil');

                UPDATE Destinations SET Region = 2, BreadcrumbRegion = 'North America'
                WHERE Slug IN ('new-york-usa', 'los-angeles-usa');

                UPDATE Destinations SET Region = 6, BreadcrumbRegion = 'Oceania'
                WHERE Slug = 'sydney-australia';

                UPDATE Destinations SET Region = 7, BreadcrumbRegion = 'Pacific Islands'
                WHERE Slug IN ('bora-bora', 'maldives');

                UPDATE Experiences SET Region = 2, BreadcrumbRegion = 'North America'
                WHERE Slug IN ('nyc-moma-after', 'aerial-canyon');

                UPDATE Stays SET Region = 2
                WHERE Slug = 'alpine-ridge-lodge';

                UPDATE Stays SET Region = 7
                WHERE Slug = 'emerald-cove-villas';

                UPDATE Destinations SET BreadcrumbRegion = 'North America' WHERE BreadcrumbRegion = 'Americas';
                UPDATE Destinations SET BreadcrumbRegion = 'Pacific Islands' WHERE BreadcrumbRegion IN ('Pacific', 'Indian Ocean');
                UPDATE Experiences SET BreadcrumbRegion = 'North America' WHERE BreadcrumbRegion = 'Americas';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE Destinations SET Region = 2, BreadcrumbRegion = 'Americas'
                WHERE Slug IN ('patagonia-wild', 'rio-de-janeiro-brazil', 'new-york-usa', 'los-angeles-usa');

                UPDATE Destinations SET Region = 1, BreadcrumbRegion = 'Oceania'
                WHERE Slug = 'sydney-australia';

                UPDATE Destinations SET Region = 1, BreadcrumbRegion = 'Indian Ocean'
                WHERE Slug = 'maldives';

                UPDATE Destinations SET Region = 1, BreadcrumbRegion = 'Pacific'
                WHERE Slug = 'bora-bora';

                UPDATE Experiences SET Region = 2, BreadcrumbRegion = 'Americas'
                WHERE Slug IN ('nyc-moma-after', 'aerial-canyon');

                UPDATE Stays SET Region = 2
                WHERE Slug IN ('alpine-ridge-lodge', 'emerald-cove-villas');
                """);
        }
    }
}
