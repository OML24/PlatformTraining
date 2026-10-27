using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZU.Training.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class QualityRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "course_cancellation_requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReasonAr = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReviewNoteAr = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    ReviewedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReviewedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OfficialDocumentStorageKey = table.Column<string>(type: "text", nullable: false),
                    OfficialDocumentOriginalName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    OfficialDocumentSha256 = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_cancellation_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_course_cancellation_requests_courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_course_cancellation_requests_user_accounts_RequestedByUserId",
                        column: x => x.RequestedByUserId,
                        principalTable: "user_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_course_cancellation_requests_user_accounts_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "user_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "official_documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Kind = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OriginalName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    StorageKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Sha256 = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    UploadedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_official_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_official_documents_user_accounts_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "user_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "training_needs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FacultyId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmittedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TitleAr = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TitleEn = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DescriptionAr = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    DescriptionEn = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReviewNoteAr = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    ReviewedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReviewedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    OfficialDocumentStorageKey = table.Column<string>(type: "text", nullable: false),
                    OfficialDocumentOriginalName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    OfficialDocumentSha256 = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_training_needs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_training_needs_faculties_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "faculties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_training_needs_user_accounts_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "user_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_training_needs_user_accounts_SubmittedByUserId",
                        column: x => x.SubmittedByUserId,
                        principalTable: "user_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "Id", "CreatedAtUtc", "Key", "NameAr", "NameEn", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df430118"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "training_needs.view", "عرض الاحتياجات التدريبية", "View training needs", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df430119"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "training_needs.submit", "رفع الاحتياجات التدريبية", "Submit training needs", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df43011a"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "training_needs.review", "مراجعة الاحتياجات التدريبية", "Review training needs", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df43011b"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "course_cancellations.view", "عرض طلبات إلغاء الدورات", "View cancellation requests", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df43011c"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "course_cancellations.request", "طلب إلغاء دورة", "Request course cancellation", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df43011d"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "course_cancellations.review", "البت في طلب إلغاء دورة", "Review course cancellation", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df43011e"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "official_documents.view", "عرض الكتب الرسمية", "View official documents", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "Id", "CreatedAtUtc", "PermissionId", "RoleId", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680001"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430118"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680002"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430118"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680003"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430119"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680004"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43011a"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680005"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43011b"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680006"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43011b"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680007"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43011c"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680008"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43011d"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680009"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43011e"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145868000a"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43011e"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_cancellation_requests_CourseId",
                table: "course_cancellation_requests",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_course_cancellation_requests_RequestedByUserId",
                table: "course_cancellation_requests",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_course_cancellation_requests_ReviewedByUserId",
                table: "course_cancellation_requests",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_official_documents_EntityType_EntityId",
                table: "official_documents",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_official_documents_UploadedByUserId",
                table: "official_documents",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_training_needs_FacultyId",
                table: "training_needs",
                column: "FacultyId");

            migrationBuilder.CreateIndex(
                name: "IX_training_needs_ReviewedByUserId",
                table: "training_needs",
                column: "ReviewedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_training_needs_SubmittedByUserId",
                table: "training_needs",
                column: "SubmittedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_cancellation_requests");

            migrationBuilder.DropTable(
                name: "official_documents");

            migrationBuilder.DropTable(
                name: "training_needs");

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680001"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680002"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680003"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680004"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680005"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680006"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680007"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680008"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680009"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145868000a"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430118"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430119"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df43011a"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df43011b"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df43011c"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df43011d"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df43011e"));
        }
    }
}
