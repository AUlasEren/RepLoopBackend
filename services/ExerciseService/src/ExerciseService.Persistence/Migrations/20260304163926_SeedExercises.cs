using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExerciseService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedExercises : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "CreatedAt", "CreatedByUserId", "Description", "Difficulty", "Equipment", "ImageUrl", "IsPublic", "MuscleGroup", "Name", "UpdatedAt", "VideoUrl" },
                values: new object[,]
                {
                    { new Guid("a0000001-0000-0000-0000-000000000001"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Chest", "Bench Press", null, null },
                    { new Guid("a0000001-0000-0000-0000-000000000002"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Chest", "Incline Bench Press", null, null },
                    { new Guid("a0000001-0000-0000-0000-000000000003"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Chest", "Decline Bench Press", null, null },
                    { new Guid("a0000001-0000-0000-0000-000000000004"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Dumbbells", null, true, "Chest", "Dumbbell Bench Press", null, null },
                    { new Guid("a0000001-0000-0000-0000-000000000005"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Dumbbells", null, true, "Chest", "Incline Dumbbell Press", null, null },
                    { new Guid("a0000001-0000-0000-0000-000000000006"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Chest", "Cable Fly", null, null },
                    { new Guid("a0000001-0000-0000-0000-000000000007"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Chest", "Dumbbell Fly", null, null },
                    { new Guid("a0000001-0000-0000-0000-000000000008"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Bodyweight", null, true, "Chest", "Push Up", null, null },
                    { new Guid("a0000001-0000-0000-0000-000000000009"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Bodyweight", null, true, "Chest", "Chest Dip", null, null },
                    { new Guid("a0000001-0000-0000-0000-00000000000a"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Chest", "Machine Chest Press", null, null },
                    { new Guid("a0000002-0000-0000-0000-000000000001"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Bodyweight", null, true, "Back", "Pull Up", null, null },
                    { new Guid("a0000002-0000-0000-0000-000000000002"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Bodyweight", null, true, "Back", "Chin Up", null, null },
                    { new Guid("a0000002-0000-0000-0000-000000000003"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Back", "Lat Pulldown", null, null },
                    { new Guid("a0000002-0000-0000-0000-000000000004"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Back", "Barbell Row", null, null },
                    { new Guid("a0000002-0000-0000-0000-000000000005"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Back", "Dumbbell Row", null, null },
                    { new Guid("a0000002-0000-0000-0000-000000000006"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Back", "Seated Cable Row", null, null },
                    { new Guid("a0000002-0000-0000-0000-000000000007"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Barbell", null, true, "Back", "Deadlift", null, null },
                    { new Guid("a0000002-0000-0000-0000-000000000008"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Back", "T-Bar Row", null, null },
                    { new Guid("a0000002-0000-0000-0000-000000000009"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Back", "Face Pull", null, null },
                    { new Guid("a0000002-0000-0000-0000-00000000000a"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Bodyweight", null, true, "Back", "Hyperextension", null, null },
                    { new Guid("a0000003-0000-0000-0000-000000000001"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Shoulders", "Overhead Press", null, null },
                    { new Guid("a0000003-0000-0000-0000-000000000002"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Dumbbells", null, true, "Shoulders", "Dumbbell Shoulder Press", null, null },
                    { new Guid("a0000003-0000-0000-0000-000000000003"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Dumbbells", null, true, "Shoulders", "Arnold Press", null, null },
                    { new Guid("a0000003-0000-0000-0000-000000000004"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Shoulders", "Lateral Raise", null, null },
                    { new Guid("a0000003-0000-0000-0000-000000000005"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Shoulders", "Front Raise", null, null },
                    { new Guid("a0000003-0000-0000-0000-000000000006"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Shoulders", "Reverse Fly", null, null },
                    { new Guid("a0000003-0000-0000-0000-000000000007"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Shoulders", "Upright Row", null, null },
                    { new Guid("a0000003-0000-0000-0000-000000000008"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Shoulders", "Cable Lateral Raise", null, null },
                    { new Guid("a0000004-0000-0000-0000-000000000001"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Barbell", null, true, "Biceps", "Barbell Curl", null, null },
                    { new Guid("a0000004-0000-0000-0000-000000000002"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Biceps", "Dumbbell Curl", null, null },
                    { new Guid("a0000004-0000-0000-0000-000000000003"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Biceps", "Hammer Curl", null, null },
                    { new Guid("a0000004-0000-0000-0000-000000000004"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Barbell", null, true, "Biceps", "Preacher Curl", null, null },
                    { new Guid("a0000004-0000-0000-0000-000000000005"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Biceps", "Concentration Curl", null, null },
                    { new Guid("a0000004-0000-0000-0000-000000000006"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Biceps", "Cable Curl", null, null },
                    { new Guid("a0000005-0000-0000-0000-000000000001"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Triceps", "Tricep Pushdown", null, null },
                    { new Guid("a0000005-0000-0000-0000-000000000002"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Triceps", "Skull Crusher", null, null },
                    { new Guid("a0000005-0000-0000-0000-000000000003"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Bodyweight", null, true, "Triceps", "Tricep Dip", null, null },
                    { new Guid("a0000005-0000-0000-0000-000000000004"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Triceps", "Overhead Tricep Extension", null, null },
                    { new Guid("a0000005-0000-0000-0000-000000000005"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Triceps", "Close Grip Bench Press", null, null },
                    { new Guid("a0000005-0000-0000-0000-000000000006"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Triceps", "Tricep Kickback", null, null },
                    { new Guid("a0000006-0000-0000-0000-000000000001"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Legs", "Barbell Squat", null, null },
                    { new Guid("a0000006-0000-0000-0000-000000000002"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Barbell", null, true, "Legs", "Front Squat", null, null },
                    { new Guid("a0000006-0000-0000-0000-000000000003"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Legs", "Leg Press", null, null },
                    { new Guid("a0000006-0000-0000-0000-000000000004"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Barbell", null, true, "Legs", "Romanian Deadlift", null, null },
                    { new Guid("a0000006-0000-0000-0000-000000000005"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Legs", "Lunge", null, null },
                    { new Guid("a0000006-0000-0000-0000-000000000006"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Dumbbells", null, true, "Legs", "Bulgarian Split Squat", null, null },
                    { new Guid("a0000006-0000-0000-0000-000000000007"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Legs", "Leg Extension", null, null },
                    { new Guid("a0000006-0000-0000-0000-000000000008"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Legs", "Leg Curl", null, null },
                    { new Guid("a0000006-0000-0000-0000-000000000009"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Legs", "Calf Raise", null, null },
                    { new Guid("a0000006-0000-0000-0000-00000000000a"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Dumbbells", null, true, "Legs", "Goblet Squat", null, null },
                    { new Guid("a0000007-0000-0000-0000-000000000001"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Bodyweight", null, true, "Core", "Plank", null, null },
                    { new Guid("a0000007-0000-0000-0000-000000000002"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Bodyweight", null, true, "Core", "Russian Twist", null, null },
                    { new Guid("a0000007-0000-0000-0000-000000000003"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Bodyweight", null, true, "Core", "Hanging Leg Raise", null, null },
                    { new Guid("a0000007-0000-0000-0000-000000000004"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Cable", null, true, "Core", "Cable Crunch", null, null },
                    { new Guid("a0000007-0000-0000-0000-000000000005"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Other", null, true, "Core", "Ab Wheel Rollout", null, null },
                    { new Guid("a0000007-0000-0000-0000-000000000006"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Bodyweight", null, true, "Core", "Mountain Climber", null, null },
                    { new Guid("a0000008-0000-0000-0000-000000000001"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Cardio", "Treadmill Run", null, null },
                    { new Guid("a0000008-0000-0000-0000-000000000002"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Cardio", "Cycling", null, null },
                    { new Guid("a0000008-0000-0000-0000-000000000003"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Other", null, true, "Cardio", "Jump Rope", null, null },
                    { new Guid("a0000008-0000-0000-0000-000000000004"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Cardio", "Rowing Machine", null, null },
                    { new Guid("a0000008-0000-0000-0000-000000000005"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Bodyweight", null, true, "Cardio", "Burpee", null, null },
                    { new Guid("a0000008-0000-0000-0000-000000000006"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Beginner", "Machine", null, true, "Cardio", "Stair Climber", null, null },
                    { new Guid("a0000009-0000-0000-0000-000000000001"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Barbell", null, true, "Full Body", "Clean and Press", null, null },
                    { new Guid("a0000009-0000-0000-0000-000000000002"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Intermediate", "Kettlebell", null, true, "Full Body", "Kettlebell Swing", null, null },
                    { new Guid("a0000009-0000-0000-0000-000000000003"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Barbell", null, true, "Full Body", "Thruster", null, null },
                    { new Guid("a0000009-0000-0000-0000-000000000004"), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Advanced", "Kettlebell", null, true, "Full Body", "Turkish Get Up", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-00000000000a"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-00000000000a"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000007"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000008"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000009"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-00000000000a"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000005"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000006"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000004"));
        }
    }
}
