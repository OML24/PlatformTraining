using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZU.Training.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CoursePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "Id", "CreatedAtUtc", "Key", "NameAr", "NameEn", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df430106"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "courses.view", "عرض الدورات", "View courses", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df430107"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "courses.create", "إنشاء الدورات", "Create courses", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df430108"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "courses.approve", "اعتماد الدورات", "Approve courses", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df430109"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "courses.self_enroll", "التسجيل الذاتي", "Self enroll", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df43010a"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "courses.participants.select", "اختيار المشاركين", "Select participants", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df43010b"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "courses.lectures.manage", "إدارة المحاضرات وأكوادها", "Manage lectures and codes", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df43010c"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "courses.attendance.enter_code", "إدخال كود الحضور", "Enter attendance code", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df43010d"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "courses.attendance.correct", "تصحيح الحضور", "Correct attendance", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df43010e"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "courses.end", "إنهاء الدورة", "End course", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df43010f"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "courses.exam.record", "توثيق الامتحان ونتائجه", "Record exam and results", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df430110"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "directory.users.search", "البحث في دليل المستخدمين", "Search user directory", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "Id", "CreatedAtUtc", "PermissionId", "RoleId", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670001"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430106"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670002"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430106"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670003"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430106"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670004"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430106"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c05"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670005"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430106"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c06"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670006"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430107"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670007"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430107"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670008"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430108"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670009"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430109"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145867000a"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430109"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145867000b"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430109"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145867000c"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430109"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c05"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145867000d"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010a"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145867000e"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010b"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145867000f"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010b"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670010"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010b"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670011"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010b"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c05"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670012"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010b"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c06"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670013"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010c"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670014"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010c"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670015"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010c"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670016"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010c"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c05"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670017"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010d"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670018"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010d"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670019"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010d"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145867001a"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010d"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c05"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145867001b"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010d"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c06"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145867001c"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010e"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145867001d"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010e"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145867001e"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010e"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145867001f"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010e"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c05"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670020"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010e"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c06"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670021"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010f"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670022"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010f"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670023"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010f"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670024"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010f"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c05"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670025"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43010f"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c06"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670026"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430110"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458670027"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430110"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670001"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670002"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670003"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670004"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670005"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670006"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670007"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670008"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670009"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145867000a"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145867000b"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145867000c"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145867000d"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145867000e"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145867000f"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670010"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670011"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670012"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670013"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670014"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670015"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670016"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670017"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670018"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670019"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145867001a"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145867001b"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145867001c"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145867001d"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145867001e"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145867001f"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670020"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670021"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670022"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670023"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670024"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670025"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670026"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458670027"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430106"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430107"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430108"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430109"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df43010a"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df43010b"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df43010c"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df43010d"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df43010e"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df43010f"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430110"));
        }
    }
}
