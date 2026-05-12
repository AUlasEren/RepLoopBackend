using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExerciseService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveExerciseVideoUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Exercises");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Exercises",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000001"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000002"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000003"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000004"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000005"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000006"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000007"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000008"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-000000000009"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-00000000000a"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-00000000000b"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-00000000000c"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-00000000000d"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-00000000000e"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000001-0000-0000-0000-00000000000f"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000001"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000002"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000003"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000004"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000005"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000006"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000007"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000008"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-000000000009"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-00000000000a"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-00000000000b"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-00000000000c"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-00000000000d"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-00000000000e"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000002-0000-0000-0000-00000000000f"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000001"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000002"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000003"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000004"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000005"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000006"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000007"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000008"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-000000000009"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-00000000000a"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-00000000000b"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-00000000000c"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-00000000000d"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-00000000000e"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000003-0000-0000-0000-00000000000f"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000001"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000002"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000003"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000004"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000005"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000006"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000007"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000008"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-000000000009"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-00000000000a"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-00000000000b"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000004-0000-0000-0000-00000000000c"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000001"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000002"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000003"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000004"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000005"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000006"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000007"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000008"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-000000000009"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-00000000000a"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-00000000000b"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000005-0000-0000-0000-00000000000c"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000001"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000002"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000003"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000004"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000005"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000006"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000007"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000008"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-000000000009"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-00000000000a"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-00000000000b"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-00000000000c"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-00000000000d"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-00000000000e"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000006-0000-0000-0000-00000000000f"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000001"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000002"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000003"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000004"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000005"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000006"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000007"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000008"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-000000000009"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-00000000000a"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-00000000000b"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000007-0000-0000-0000-00000000000c"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000001"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000002"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000003"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000004"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000005"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000006"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000007"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000008"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-000000000009"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000008-0000-0000-0000-00000000000a"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000001"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000002"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000003"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000004"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000005"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000006"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000007"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000008"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-000000000009"),
                column: "VideoUrl",
                value: null);

            migrationBuilder.UpdateData(
                table: "Exercises",
                keyColumn: "Id",
                keyValue: new Guid("a0000009-0000-0000-0000-00000000000a"),
                column: "VideoUrl",
                value: null);
        }
    }
}
