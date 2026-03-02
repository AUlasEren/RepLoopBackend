using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SettingsService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVisibilityFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileVisibility",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "WorkoutVisibility",
                table: "UserSettings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileVisibility",
                table: "UserSettings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WorkoutVisibility",
                table: "UserSettings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
