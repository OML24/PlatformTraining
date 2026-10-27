using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZU.Training.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExamRequirementChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_exams_CourseId",
                table: "exams");

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "Id", "CreatedAtUtc", "Key", "NameAr", "NameEn", "UpdatedAtUtc" },
                values: new object[] { new Guid("8e9838fb-c639-4adc-a40a-a926df430124"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "courses.exam_requirement.change", "تعديل شرط امتحان الدورة", "Change course exam requirement", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "Id", "CreatedAtUtc", "PermissionId", "RoleId", "UpdatedAtUtc" },
                values: new object[] { new Guid("c0df69bb-93b9-43ce-8812-32145868001a"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430124"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_exams_CourseId",
                table: "exams",
                column: "CourseId",
                unique: true,
                filter: "\"IsActive\" = TRUE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_exams_CourseId",
                table: "exams");

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145868001a"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430124"));

            migrationBuilder.CreateIndex(
                name: "IX_exams_CourseId",
                table: "exams",
                column: "CourseId",
                unique: true);
        }
    }
}
