using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SettingsService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    WeightUnit = table.Column<string>(type: "text", nullable: false),
                    DistanceUnit = table.Column<string>(type: "text", nullable: false),
                    DefaultDurationMinutes = table.Column<int>(type: "integer", nullable: false),
                    RestBetweenSetsSeconds = table.Column<int>(type: "integer", nullable: false),
                    WorkoutDays = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, defaultValue: "Monday,Wednesday,Friday"),
                    EmailNotifications = table.Column<bool>(type: "boolean", nullable: false),
                    PushNotifications = table.Column<bool>(type: "boolean", nullable: false),
                    WorkoutReminders = table.Column<bool>(type: "boolean", nullable: false),
                    WeeklyReport = table.Column<bool>(type: "boolean", nullable: false),
                    AchievementAlerts = table.Column<bool>(type: "boolean", nullable: false),
                    ProfileVisibility = table.Column<string>(type: "text", nullable: false),
                    WorkoutVisibility = table.Column<string>(type: "text", nullable: false),
                    AllowDataAnalysis = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
