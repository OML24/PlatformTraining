using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZU.Training.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UniversitySyncFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SourceLastSeenAtUtc",
                table: "user_accounts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceMainJobCode",
                table: "user_accounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "SourceMissingSinceUtc",
                table: "user_accounts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SourceSideCode",
                table: "user_accounts",
                type: "integer",
                nullable: true);

            migrationBuilder.InsertData(
                table: "field_hints",
                columns: new[] { "Id", "CreatedAtUtc", "FieldKey", "ScreenKey", "TextAr", "TextEn", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940025"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "userSearch", "admin", "ابحث باسم الشخص أو رقمه الوظيفي أو رقم دخوله.", "Search by name, employee number, or login.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940026"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "employeeId", "admin", "رقم الموظف الوظيفي الفريد.", "Unique employee number.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940027"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "externalLogin", "admin", "أربعة أرقام فريدة؛ اتركه فارغًا لتوليد رقم تلقائي.", "Unique four digit number; leave blank to generate one.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940028"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "nameAr", "admin", "الاسم العربي المطلوب في الحساب.", "Required Arabic account name.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940029"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "nameEn", "admin", "الاسم الإنجليزي اختياري.", "English account name is optional.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594002a"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "email", "admin", "البريد المعتمد للتواصل مع صاحب الحساب.", "Approved account contact email.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594002b"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "faculty", "admin", "الكلية التي ينتمي إليها الموظف عند توفر الربط الرسمي.", "Faculty when official mapping is available.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594002c"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "externalPassword", "admin", "كلمة مرور المحاضر الخارجي التي يحددها الأدمن.", "Administrator-set external trainer password.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594002d"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "newPassword", "admin", "تغيير كلمة المرور الحالية لأي حساب.", "Change the current password for any account.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594002e"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "sharedPassword", "admin", "كلمة أولية بديلة للحسابات قبل إكمال أول دخول.", "Fallback password for accounts before first login.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "Id", "CreatedAtUtc", "Key", "NameAr", "NameEn", "UpdatedAtUtc" },
                values: new object[] { new Guid("8e9838fb-c639-4adc-a40a-a926df430117"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "admin.university_sync.run", "تحديث بيانات الجامعة", "Sync university directory", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940025"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940026"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940027"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940028"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940029"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594002a"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594002b"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594002c"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594002d"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594002e"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430117"));

            migrationBuilder.DropColumn(
                name: "SourceLastSeenAtUtc",
                table: "user_accounts");

            migrationBuilder.DropColumn(
                name: "SourceMainJobCode",
                table: "user_accounts");

            migrationBuilder.DropColumn(
                name: "SourceMissingSinceUtc",
                table: "user_accounts");

            migrationBuilder.DropColumn(
                name: "SourceSideCode",
                table: "user_accounts");
        }
    }
}
