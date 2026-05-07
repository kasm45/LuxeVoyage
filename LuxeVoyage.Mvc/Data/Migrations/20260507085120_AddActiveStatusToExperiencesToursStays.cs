using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxeVoyage.Mvc.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveStatusToExperiencesToursStays : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tours",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Stays",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Experiences",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.Sql("UPDATE Tours SET IsActive = 1 WHERE Id IS NOT NULL;");
            migrationBuilder.Sql("UPDATE Stays SET IsActive = 1 WHERE Id IS NOT NULL;");
            migrationBuilder.Sql("UPDATE Experiences SET IsActive = 1 WHERE Id IS NOT NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Experiences");
        }
    }
}
