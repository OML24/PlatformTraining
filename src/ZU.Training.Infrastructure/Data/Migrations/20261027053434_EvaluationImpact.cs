using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZU.Training.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EvaluationImpact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EvaluationFormVersionId",
                table: "courses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "evaluation_form_versions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    PublishedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PublishedByUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation_form_versions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_evaluation_form_versions_user_accounts_PublishedByUserId",
                        column: x => x.PublishedByUserId,
                        principalTable: "user_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "impact_submissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    OriginalName = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    StorageKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Sha256 = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ReviewNoteAr = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    ReviewedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReviewedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_impact_submissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_impact_submissions_course_enrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "course_enrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_impact_submissions_user_accounts_ReviewedByUserId",
                        column: x => x.ReviewedByUserId,
                        principalTable: "user_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "evaluation_questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FormVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    TextAr = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TextEn = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation_questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_evaluation_questions_evaluation_form_versions_FormVersionId",
                        column: x => x.FormVersionId,
                        principalTable: "evaluation_form_versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "evaluation_responses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FormVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubmittedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation_responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_evaluation_responses_course_enrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "course_enrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_evaluation_responses_evaluation_form_versions_FormVersionId",
                        column: x => x.FormVersionId,
                        principalTable: "evaluation_form_versions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "evaluation_options",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<int>(type: "integer", nullable: false),
                    TextAr = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TextEn = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation_options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_evaluation_options_evaluation_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "evaluation_questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "evaluation_answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResponseId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    OptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evaluation_answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_evaluation_answers_evaluation_options_OptionId",
                        column: x => x.OptionId,
                        principalTable: "evaluation_options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_evaluation_answers_evaluation_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "evaluation_questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_evaluation_answers_evaluation_responses_ResponseId",
                        column: x => x.ResponseId,
                        principalTable: "evaluation_responses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "permissions",
                columns: new[] { "Id", "CreatedAtUtc", "Key", "NameAr", "NameEn", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df43011f"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "impact.submit", "رفع الأثر التدريبي", "Submit training impact", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df430120"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "impact.review", "مراجعة الأثر التدريبي", "Review training impact", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df430121"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "evaluation.manage", "إدارة نموذج التقييم", "Manage evaluation form", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df430122"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "evaluation.submit", "تعبئة التقييم", "Submit evaluation", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("8e9838fb-c639-4adc-a40a-a926df430123"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "evaluation.statistics.view", "عرض إحصاءات التقييم", "View evaluation statistics", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "Id", "CreatedAtUtc", "PermissionId", "RoleId", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("c0df69bb-93b9-43ce-8812-32145868000b"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43011f"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145868000c"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43011f"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145868000d"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43011f"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145868000e"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df43011f"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c05"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-32145868000f"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430120"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680010"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430120"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680011"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430120"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c06"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680012"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430121"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680013"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430122"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680014"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430122"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c03"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680015"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430122"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680016"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430122"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c05"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680017"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430123"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c02"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680018"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430123"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c04"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("c0df69bb-93b9-43ce-8812-321458680019"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("8e9838fb-c639-4adc-a40a-a926df430123"), new Guid("5c98724b-cead-42f7-b20d-638817eb3c06"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_courses_EvaluationFormVersionId",
                table: "courses",
                column: "EvaluationFormVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_answers_OptionId",
                table: "evaluation_answers",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_answers_QuestionId",
                table: "evaluation_answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_answers_ResponseId_QuestionId",
                table: "evaluation_answers",
                columns: new[] { "ResponseId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_form_versions_PublishedByUserId",
                table: "evaluation_form_versions",
                column: "PublishedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_form_versions_Version",
                table: "evaluation_form_versions",
                column: "Version",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_options_QuestionId_Position",
                table: "evaluation_options",
                columns: new[] { "QuestionId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_questions_FormVersionId_Position",
                table: "evaluation_questions",
                columns: new[] { "FormVersionId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_responses_EnrollmentId",
                table: "evaluation_responses",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_evaluation_responses_FormVersionId_EnrollmentId",
                table: "evaluation_responses",
                columns: new[] { "FormVersionId", "EnrollmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_impact_submissions_EnrollmentId_Version",
                table: "impact_submissions",
                columns: new[] { "EnrollmentId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_impact_submissions_ReviewedByUserId",
                table: "impact_submissions",
                column: "ReviewedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_evaluation_form_versions_EvaluationFormVersionId",
                table: "courses",
                column: "EvaluationFormVersionId",
                principalTable: "evaluation_form_versions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_evaluation_form_versions_EvaluationFormVersionId",
                table: "courses");

            migrationBuilder.DropTable(
                name: "evaluation_answers");

            migrationBuilder.DropTable(
                name: "impact_submissions");

            migrationBuilder.DropTable(
                name: "evaluation_options");

            migrationBuilder.DropTable(
                name: "evaluation_responses");

            migrationBuilder.DropTable(
                name: "evaluation_questions");

            migrationBuilder.DropTable(
                name: "evaluation_form_versions");

            migrationBuilder.DropIndex(
                name: "IX_courses_EvaluationFormVersionId",
                table: "courses");

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145868000b"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145868000c"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145868000d"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145868000e"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-32145868000f"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680010"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680011"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680012"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680013"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680014"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680015"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680016"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680017"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680018"));

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumn: "Id",
                keyValue: new Guid("c0df69bb-93b9-43ce-8812-321458680019"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df43011f"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430120"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430121"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430122"));

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Id",
                keyValue: new Guid("8e9838fb-c639-4adc-a40a-a926df430123"));

            migrationBuilder.DropColumn(
                name: "EvaluationFormVersionId",
                table: "courses");
        }
    }
}
