using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxeVoyage.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRichCmsFieldsForExperiencesToursStays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancellationPolicy",
                table: "Tours",
                type: "TEXT",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardBadge",
                table: "Tours",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardImageUrl",
                table: "Tours",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardPriceHint",
                table: "Tours",
                type: "TEXT",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CardRating",
                table: "Tours",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardRegion",
                table: "Tours",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardSummary",
                table: "Tours",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardTitle",
                table: "Tours",
                type: "TEXT",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationLabel",
                table: "Tours",
                type: "TEXT",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailDescription",
                table: "Tours",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailTagline",
                table: "Tours",
                type: "TEXT",
                maxLength: 220,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage1Url",
                table: "Tours",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage2Url",
                table: "Tours",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage3Url",
                table: "Tours",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage4Url",
                table: "Tours",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupSizeText",
                table: "Tours",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeroImageUrl",
                table: "Tours",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeroSubtitle",
                table: "Tours",
                type: "TEXT",
                maxLength: 220,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeroTitle",
                table: "Tours",
                type: "TEXT",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighlightsCsv",
                table: "Tours",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncludedItemsCsv",
                table: "Tours",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleOnListing",
                table: "Tours",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "ItineraryText",
                table: "Tours",
                type: "TEXT",
                maxLength: 6000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Region",
                table: "Tours",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WhatToBring",
                table: "Tours",
                type: "TEXT",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLabel",
                table: "Stays",
                type: "TEXT",
                maxLength: 220,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AmenitiesDetailCsv",
                table: "Stays",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BedInfo",
                table: "Stays",
                type: "TEXT",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancellationPolicy",
                table: "Stays",
                type: "TEXT",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardBadge",
                table: "Stays",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardImageUrl",
                table: "Stays",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardPriceHint",
                table: "Stays",
                type: "TEXT",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CardRating",
                table: "Stays",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardRegion",
                table: "Stays",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardSummary",
                table: "Stays",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardTitle",
                table: "Stays",
                type: "TEXT",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CheckInInfo",
                table: "Stays",
                type: "TEXT",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailDescription",
                table: "Stays",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailTagline",
                table: "Stays",
                type: "TEXT",
                maxLength: 220,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage1Url",
                table: "Stays",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage2Url",
                table: "Stays",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage3Url",
                table: "Stays",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage4Url",
                table: "Stays",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestCapacity",
                table: "Stays",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeroImageUrl",
                table: "Stays",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeroSubtitle",
                table: "Stays",
                type: "TEXT",
                maxLength: 220,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeroTitle",
                table: "Stays",
                type: "TEXT",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighlightsCsv",
                table: "Stays",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleOnListing",
                table: "Stays",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "NearbyAttractionsCsv",
                table: "Stays",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomType",
                table: "Stays",
                type: "TEXT",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StayType",
                table: "Stays",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardBadge",
                table: "Experiences",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardImageUrl",
                table: "Experiences",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardPriceHint",
                table: "Experiences",
                type: "TEXT",
                maxLength: 60,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CardRating",
                table: "Experiences",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardRegion",
                table: "Experiences",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardSummary",
                table: "Experiences",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardTitle",
                table: "Experiences",
                type: "TEXT",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailDescription",
                table: "Experiences",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DetailTagline",
                table: "Experiences",
                type: "TEXT",
                maxLength: 220,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DurationText",
                table: "Experiences",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage1Url",
                table: "Experiences",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage2Url",
                table: "Experiences",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage3Url",
                table: "Experiences",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GalleryImage4Url",
                table: "Experiences",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroupSizeText",
                table: "Experiences",
                type: "TEXT",
                maxLength: 80,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeroImageUrl",
                table: "Experiences",
                type: "TEXT",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeroSubtitle",
                table: "Experiences",
                type: "TEXT",
                maxLength: 220,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeroTitle",
                table: "Experiences",
                type: "TEXT",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HighlightsCsv",
                table: "Experiences",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IncludedItemsCsv",
                table: "Experiences",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleOnListing",
                table: "Experiences",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "ItineraryText",
                table: "Experiences",
                type: "TEXT",
                maxLength: 6000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeetingPoint",
                table: "Experiences",
                type: "TEXT",
                maxLength: 160,
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE Experiences
                SET CardTitle = Title,
                    CardSummary = Summary,
                    CardImageUrl = ImageUrl,
                    CardPriceHint = PriceHint,
                    CardRating = Rating,
                    HeroTitle = Title,
                    DetailDescription = Summary,
                    IsVisibleOnListing = 1
                WHERE Id IS NOT NULL;
                """);

            migrationBuilder.Sql(
                """
                UPDATE Tours
                SET CardTitle = Title,
                    CardSummary = Summary,
                    CardImageUrl = ImageUrl,
                    CardPriceHint = '$' || CAST(Price AS TEXT),
                    CardRating = Rating,
                    HeroTitle = Title,
                    DetailDescription = Summary,
                    IsVisibleOnListing = 1
                WHERE Id IS NOT NULL;
                """);

            migrationBuilder.Sql(
                """
                UPDATE Stays
                SET CardTitle = Name,
                    CardSummary = CityLine,
                    CardImageUrl = ImageUrl,
                    CardPriceHint = '$' || CAST(PricePerNight AS TEXT) || ' / night',
                    CardRating = StarRating,
                    HeroTitle = Name,
                    DetailDescription = CityLine,
                    IsVisibleOnListing = 1
                WHERE Id IS NOT NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancellationPolicy",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "CardBadge",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "CardImageUrl",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "CardPriceHint",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "CardRating",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "CardRegion",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "CardSummary",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "CardTitle",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "DestinationLabel",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "DetailDescription",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "DetailTagline",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "GalleryImage1Url",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "GalleryImage2Url",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "GalleryImage3Url",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "GalleryImage4Url",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "GroupSizeText",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "HeroImageUrl",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "HeroSubtitle",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "HeroTitle",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "HighlightsCsv",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "IncludedItemsCsv",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "IsVisibleOnListing",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "ItineraryText",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "WhatToBring",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "AddressLabel",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "AmenitiesDetailCsv",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "BedInfo",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "CancellationPolicy",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "CardBadge",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "CardImageUrl",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "CardPriceHint",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "CardRating",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "CardRegion",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "CardSummary",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "CardTitle",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "CheckInInfo",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "DetailDescription",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "DetailTagline",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "GalleryImage1Url",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "GalleryImage2Url",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "GalleryImage3Url",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "GalleryImage4Url",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "GuestCapacity",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "HeroImageUrl",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "HeroSubtitle",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "HeroTitle",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "HighlightsCsv",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "IsVisibleOnListing",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "NearbyAttractionsCsv",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "RoomType",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "StayType",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "CardBadge",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "CardImageUrl",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "CardPriceHint",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "CardRating",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "CardRegion",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "CardSummary",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "CardTitle",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "DetailDescription",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "DetailTagline",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "DurationText",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "GalleryImage1Url",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "GalleryImage2Url",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "GalleryImage3Url",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "GalleryImage4Url",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "GroupSizeText",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "HeroImageUrl",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "HeroSubtitle",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "HeroTitle",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "HighlightsCsv",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "IncludedItemsCsv",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "IsVisibleOnListing",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "ItineraryText",
                table: "Experiences");

            migrationBuilder.DropColumn(
                name: "MeetingPoint",
                table: "Experiences");
        }
    }
}
