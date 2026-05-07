using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxeVoyage.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class CurateSeedCatalogActivation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE Destinations SET IsActive = 0
                WHERE Slug IN (
                    'athens-greece', 'bangkok-thailand', 'cairo-egypt', 'cape-town-south-africa',
                    'los-angeles-usa', 'marrakech-medina', 'amalfi-coast', 'patagonia-wild',
                    'prague-czech-republic', 'zermatt', 'antalya-turkey'
                );

                UPDATE Destinations SET IsActive = 1
                WHERE Slug IN (
                    'amsterdam-netherlands', 'bali-indonesia', 'barcelona-spain', 'bora-bora',
                    'cappadocia-turkey', 'dubai-uae', 'istanbul-turkey', 'kyoto-quarter',
                    'london-uk', 'maldives', 'new-york-usa', 'paris-france', 'rio-de-janeiro-brazil',
                    'rome-italy', 'santorini-greece', 'swiss-alps', 'sydney-australia', 'tokyo'
                );

                UPDATE Experiences SET IsActive = 0 WHERE Slug IN ('aerial-canyon');
                UPDATE Experiences SET IsActive = 1
                WHERE Slug IN (
                    'dubai-dune-safari', 'fushimi-inari-twilight', 'holistic-wellness',
                    'nyc-moma-after', 'private-vineyard', 'alpine-sanctuary', 'parisian-gallery'
                );

                UPDATE Tours SET IsActive = 0 WHERE Slug = 'grand-canyon-expedition';
                UPDATE Tours SET IsActive = 1
                WHERE Slug IN (
                    'bali-wellness-escape', 'kyoto-heritage-walk', 'maldives-retreat',
                    'parisian-romance', 'swiss-alps-express'
                );

                UPDATE Stays SET IsActive = 0 WHERE Slug = 'alpine-ridge-lodge';
                UPDATE Stays SET IsActive = 1
                WHERE Slug IN (
                    'azure-horizon-resort', 'emerald-cove-villas', 'jungle-canopy-retreat',
                    'grand-imperial', 'metropolis-suites'
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE Destinations SET IsActive = 1
                WHERE Slug IN (
                    'athens-greece', 'bangkok-thailand', 'cairo-egypt', 'cape-town-south-africa',
                    'los-angeles-usa', 'marrakech-medina', 'amalfi-coast', 'patagonia-wild',
                    'prague-czech-republic', 'zermatt', 'antalya-turkey'
                );

                UPDATE Experiences SET IsActive = 1 WHERE Slug = 'aerial-canyon';
                UPDATE Tours SET IsActive = 1 WHERE Slug = 'grand-canyon-expedition';
                UPDATE Stays SET IsActive = 1 WHERE Slug = 'alpine-ridge-lodge';
                """);
        }
    }
}
