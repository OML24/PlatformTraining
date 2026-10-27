using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZU.Training.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EvaluationImpactHints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "field_hints",
                columns: new[] { "Id", "CreatedAtUtc", "FieldKey", "ScreenKey", "TextAr", "TextEn", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594002f"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "questionAr", "evaluation", "اكتب السؤال الرسمي بالعربية؛ سيظهر في النموذج الموحد.", "Enter the official Arabic question.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940030"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "questionEn", "evaluation", "ترجمة السؤال اختيارية.", "English question is optional.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940031"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "optionAr", "evaluation", "اكتب نص خيار الإجابة بالعربية.", "Enter the Arabic answer option.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940032"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "optionEn", "evaluation", "ترجمة الخيار اختيارية.", "English option is optional.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940033"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "file", "impact", "ارفع نسخة PDF أو DOCX جديدة لا تزيد عن 20 ميغابايت.", "Upload a revised PDF or DOCX up to 20 MB.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594002f"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940030"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940031"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940032"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940033"));
        }
    }
}
