using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZU.Training.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "password_policies",
                columns: new[] { "Id", "AllowSpaces", "BlockCommonPasswords", "CreatedAtUtc", "EffectiveAtUtc", "MaximumLength", "MinimumLength", "RequireMixedCharacterClasses", "UpdatedAtUtc" },
                values: new object[] { new Guid("f47613db-7bd5-4d19-a8fa-9e67c247c2f7"), true, true, new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 128, 15, false, new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "CreatedAtUtc", "Key", "NameAr", "NameEn", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("5c98724b-cead-42f7-b20d-638817eb3c01"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "ADMIN", "مسؤول النظام", "Administrator", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "QA", "الجودة", "Quality Assurance", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "DEAN", "عميد الكلية", "Dean", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "INST", "المحاضر", "Instructor", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("5c98724b-cead-42f7-b20d-638817eb3c05"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "MEMBER", "عضو هيئة التدريس", "Faculty Member", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("5c98724b-cead-42f7-b20d-638817eb3c06"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "EXTERNAL_INSTRUCTOR", "محاضر خارجي", "External Instructor", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "password_policies",
                keyColumn: "Id",
                keyValue: new Guid("f47613db-7bd5-4d19-a8fa-9e67c247c2f7"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("5c98724b-cead-42f7-b20d-638817eb3c01"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("5c98724b-cead-42f7-b20d-638817eb3c05"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("5c98724b-cead-42f7-b20d-638817eb3c06"));
        }
    }
}
