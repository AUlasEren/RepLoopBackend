using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExerciseService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExpandExerciseSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "Description", "Difficulty", "Equipment", "ImageUrl", "IsPublic", "MuscleGroup", "Name", "UpdatedAt", "VideoUrl" },
                values: new object[,]
                {
                    { new Guid("a0000001-0000-0000-0000-00000000000b"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Chest", "Pec Deck Machine", null, null },
                    { new Guid("a0000001-0000-0000-0000-00000000000c"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Chest", "Landmine Press", null, null },
                    { new Guid("a0000001-0000-0000-0000-00000000000d"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Dumbbells", null, true, "Chest", "Dumbbell Pullover", null, null },
                    { new Guid("a0000001-0000-0000-0000-00000000000e"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Other", null, true, "Chest", "Svend Press", null, null },
                    { new Guid("a0000001-0000-0000-0000-00000000000f"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Bodyweight", null, true, "Chest", "Diamond Push Up", null, null },
                    { new Guid("a0000002-0000-0000-0000-00000000000b"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Barbell", null, true, "Back", "Pendlay Row", null, null },
                    { new Guid("a0000002-0000-0000-0000-00000000000c"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Back", "Meadows Row", null, null },
                    { new Guid("a0000002-0000-0000-0000-00000000000d"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Back", "Straight Arm Pulldown", null, null },
                    { new Guid("a0000002-0000-0000-0000-00000000000e"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Bodyweight", null, true, "Back", "Inverted Row", null, null },
                    { new Guid("a0000002-0000-0000-0000-00000000000f"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Back", "Single Arm Cable Row", null, null },
                    { new Guid("a0000003-0000-0000-0000-000000000009"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Shoulders", "Machine Shoulder Press", null, null },
                    { new Guid("a0000003-0000-0000-0000-00000000000a"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Barbell", null, true, "Shoulders", "Behind The Neck Press", null, null },
                    { new Guid("a0000003-0000-0000-0000-00000000000b"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Dumbbells", null, true, "Shoulders", "Lu Raise", null, null },
                    { new Guid("a0000003-0000-0000-0000-00000000000c"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Shoulders", "Cable Face Pull", null, null },
                    { new Guid("a0000003-0000-0000-0000-00000000000d"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Shoulders", "Dumbbell Shrug", null, null },
                    { new Guid("a0000003-0000-0000-0000-00000000000e"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Barbell", null, true, "Shoulders", "Barbell Shrug", null, null },
                    { new Guid("a0000003-0000-0000-0000-00000000000f"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Other", null, true, "Shoulders", "Plate Front Raise", null, null },
                    { new Guid("a0000004-0000-0000-0000-000000000007"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Barbell", null, true, "Biceps", "EZ Bar Curl", null, null },
                    { new Guid("a0000004-0000-0000-0000-000000000008"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Dumbbells", null, true, "Biceps", "Incline Dumbbell Curl", null, null },
                    { new Guid("a0000004-0000-0000-0000-000000000009"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Dumbbells", null, true, "Biceps", "Spider Curl", null, null },
                    { new Guid("a0000004-0000-0000-0000-00000000000a"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Barbell", null, true, "Biceps", "Reverse Curl", null, null },
                    { new Guid("a0000004-0000-0000-0000-00000000000b"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Cable", null, true, "Biceps", "Bayesian Curl", null, null },
                    { new Guid("a0000004-0000-0000-0000-00000000000c"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Dumbbells", null, true, "Biceps", "Zottman Curl", null, null },
                    { new Guid("a0000005-0000-0000-0000-000000000007"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Triceps", "Cable Overhead Extension", null, null },
                    { new Guid("a0000005-0000-0000-0000-000000000008"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Bodyweight", null, true, "Triceps", "Bodyweight Tricep Extension", null, null },
                    { new Guid("a0000005-0000-0000-0000-000000000009"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Barbell", null, true, "Triceps", "JM Press", null, null },
                    { new Guid("a0000005-0000-0000-0000-00000000000a"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Triceps", "French Press", null, null },
                    { new Guid("a0000005-0000-0000-0000-00000000000b"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Triceps", "Rope Pushdown", null, null },
                    { new Guid("a0000005-0000-0000-0000-00000000000c"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Bodyweight", null, true, "Triceps", "Bench Dip", null, null },
                    { new Guid("a0000006-0000-0000-0000-00000000000b"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Machine", null, true, "Legs", "Hack Squat", null, null },
                    { new Guid("a0000006-0000-0000-0000-00000000000c"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Legs", "Hip Thrust", null, null },
                    { new Guid("a0000006-0000-0000-0000-00000000000d"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Barbell", null, true, "Legs", "Sumo Deadlift", null, null },
                    { new Guid("a0000006-0000-0000-0000-00000000000e"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Bodyweight", null, true, "Legs", "Sissy Squat", null, null },
                    { new Guid("a0000006-0000-0000-0000-00000000000f"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Legs", "Seated Calf Raise", null, null },
                    { new Guid("a0000007-0000-0000-0000-000000000007"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Bodyweight", null, true, "Core", "Dead Bug", null, null },
                    { new Guid("a0000007-0000-0000-0000-000000000008"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Core", "Pallof Press", null, null },
                    { new Guid("a0000007-0000-0000-0000-000000000009"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Bodyweight", null, true, "Core", "Bicycle Crunch", null, null },
                    { new Guid("a0000007-0000-0000-0000-00000000000a"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Bodyweight", null, true, "Core", "Side Plank", null, null },
                    { new Guid("a0000007-0000-0000-0000-00000000000b"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Bodyweight", null, true, "Core", "Dragon Flag", null, null },
                    { new Guid("a0000007-0000-0000-0000-00000000000c"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Cable", null, true, "Core", "Woodchopper", null, null },
                    { new Guid("a0000008-0000-0000-0000-000000000007"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Other", null, true, "Cardio", "Battle Ropes", null, null },
                    { new Guid("a0000008-0000-0000-0000-000000000008"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Other", null, true, "Cardio", "Box Jump", null, null },
                    { new Guid("a0000008-0000-0000-0000-000000000009"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Cardio", "Elliptical", null, null },
                    { new Guid("a0000008-0000-0000-0000-00000000000a"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Bodyweight", null, true, "Cardio", "High Knees", null, null },
                    { new Guid("a0000009-0000-0000-0000-000000000005"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Barbell", null, true, "Full Body", "Snatch", null, null },
                    { new Guid("a0000009-0000-0000-0000-000000000006"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Dumbbells", null, true, "Full Body", "Man Maker", null, null },
                    { new Guid("a0000009-0000-0000-0000-000000000007"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Bodyweight", null, true, "Full Body", "Bear Crawl", null, null },
                    { new Guid("a0000009-0000-0000-0000-000000000008"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Dumbbells", null, true, "Full Body", "Devil Press", null, null },
                    { new Guid("a0000009-0000-0000-0000-000000000009"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Full Body", "Farmers Walk", null, null },
                    { new Guid("a0000009-0000-0000-0000-00000000000a"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Kettlebell", null, true, "Full Body", "Kettlebell Clean", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-00000000000b"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-00000000000c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-00000000000d"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-00000000000e"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-00000000000f"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-00000000000b"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-00000000000c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-00000000000d"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-00000000000e"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-00000000000f"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-00000000000a"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-00000000000b"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-00000000000c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-00000000000d"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-00000000000e"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-00000000000f"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-00000000000a"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-00000000000b"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-00000000000c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-00000000000a"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-00000000000b"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-00000000000c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-00000000000b"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-00000000000c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-00000000000d"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-00000000000e"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-00000000000f"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-00000000000a"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-00000000000b"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-00000000000c"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-00000000000a"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-00000000000a"));
        }
    }
}
