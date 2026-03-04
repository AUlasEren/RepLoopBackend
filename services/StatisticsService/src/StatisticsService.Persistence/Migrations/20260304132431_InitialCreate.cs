using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StatisticsService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BodyMeasurements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MeasuredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    WeightKg = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    BodyFatPercentage = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    ChestCm = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    WaistCm = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    HipsCm = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    BicepsCm = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    ThighCm = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyMeasurements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExerciseName = table.Column<string>(type: "text", nullable: false),
                    WeightKg = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Reps = table.Column<int>(type: "integer", nullable: false),
                    PerformedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BodyMeasurements_UserId_MeasuredAt",
                table: "BodyMeasurements",
                columns: new[] { "UserId", "MeasuredAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_UserId_ExerciseId_PerformedAt",
                table: "ExerciseLogs",
                columns: new[] { "UserId", "ExerciseId", "PerformedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BodyMeasurements");

            migrationBuilder.DropTable(
                name: "ExerciseLogs");
        }
    }
}
