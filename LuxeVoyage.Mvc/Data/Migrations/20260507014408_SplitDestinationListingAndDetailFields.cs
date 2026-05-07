using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxeVoyage.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class SplitDestinationListingAndDetailFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardBadge",
                table: "Destinations",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardImageUrl",
                table: "Destinations",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardPriceHint",
                table: "Destinations",
                type: "TEXT",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CardRating",
                table: "Destinations",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardRegion",
                table: "Destinations",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardSummary",
                table: "Destinations",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardTitle",
                table: "Destinations",
                type: "TEXT",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailDescription",
                table: "Destinations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailTagline",
                table: "Destinations",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage1Url",
                table: "Destinations",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage2Url",
                table: "Destinations",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage3Url",
                table: "Destinations",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage4Url",
                table: "Destinations",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeroSubtitle",
                table: "Destinations",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeroTitle",
                table: "Destinations",
                type: "TEXT",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleOnListing",
                table: "Destinations",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.Sql(
                """
                UPDATE Destinations
                SET CardTitle = Title,
                    CardSummary = Summary,
                    CardImageUrl = ImageUrl,
                    CardPriceHint = PriceHint,
                    CardRating = Rating,
                    IsVisibleOnListing = 1,
                    HeroTitle = Title,
                    HeroSubtitle = CASE
                        WHEN BreadcrumbCity IS NULL OR trim(BreadcrumbCity) = '' THEN NULL
                        ELSE BreadcrumbCity
                    END,
                    DetailDescription = LongDescription,
                    DetailTagline = TagLine,
                    GalleryImage1Url = CASE
                        WHEN HeroImageUrl IS NOT NULL AND length(trim(HeroImageUrl)) > 0 THEN HeroImageUrl
                        ELSE ImageUrl
                    END
                WHERE Id IS NOT NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardBadge",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "CardImageUrl",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "CardPriceHint",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "CardRating",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "CardRegion",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "CardSummary",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "CardTitle",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "DetailDescription",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "DetailTagline",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "GalleryImage1Url",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "GalleryImage2Url",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "GalleryImage3Url",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "GalleryImage4Url",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "HeroSubtitle",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "HeroTitle",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "IsVisibleOnListing",
                table: "Destinations");
        }
    }
}
