using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxeVoyage.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDestinationDetailManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BestTimeToVisit",
                table: "Destinations",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImagesCsv",
                table: "Destinations",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeroImageUrl",
                table: "Destinations",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighlightsCsv",
                table: "Destinations",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Destinations",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "Destinations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MapImageUrl",
                table: "Destinations",
                type: "TEXT",
                maxLength: 600,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeatherClimate",
                table: "Destinations",
                type: "TEXT",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhereYoullBeText",
                table: "Destinations",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BestTimeToVisit",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "GalleryImagesCsv",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "HeroImageUrl",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "HighlightsCsv",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "MapImageUrl",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "WeatherClimate",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "WhereYoullBeText",
                table: "Destinations");
        }
    }
}
