using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZU.Training.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class CourseProposalDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartsAtUtc",
                table: "lectures",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeliveryMode",
                table: "courses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ObjectivesAr",
                table: "courses",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ObjectivesEn",
                table: "courses",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PlannedEndUtc",
                table: "courses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<decimal>(
                name: "PlannedHours",
                table: "courses",
                type: "numeric(7,2)",
                precision: 7,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "PlannedStartUtc",
                table: "courses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "TargetAudienceAr",
                table: "courses",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetAudienceEn",
                table: "courses",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TrainingTrackId",
                table: "courses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "training_tracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NameAr = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    NameEn = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    ObjectivesAr = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    ObjectivesEn = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsArchived = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_training_tracks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "training_tracks",
                columns: new[] { "Id", "CreatedAtUtc", "NameAr", "NameEn", "ObjectivesAr", "ObjectivesEn", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("b159551c-5440-46b2-9d6e-c87559dc5501"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "التطوير التقني والرقمي", "Technology and Digital Development", "تنمية المهارات التقنية والرقمية", null, new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("b159551c-5440-46b2-9d6e-c87559dc5502"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "طرائق التدريس والقياس والتقويم", "Teaching, Measurement and Assessment", "تطوير الممارسات التدريسية والتقويم", null, new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("b159551c-5440-46b2-9d6e-c87559dc5503"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "البحث العلمي والنشر الدولي", "Scientific Research and International Publishing", "تعزيز البحث العلمي والنشر", null, new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("b159551c-5440-46b2-9d6e-c87559dc5504"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "القيادة المؤسسية والموارد البشرية", "Institutional Leadership and Human Resources", "تطوير القيادة والإدارة المؤسسية", null, new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_courses_TrainingTrackId",
                table: "courses",
                column: "TrainingTrackId");

            migrationBuilder.AddForeignKey(
                name: "FK_courses_training_tracks_TrainingTrackId",
                table: "courses",
                column: "TrainingTrackId",
                principalTable: "training_tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_courses_training_tracks_TrainingTrackId",
                table: "courses");

            migrationBuilder.DropTable(
                name: "training_tracks");

            migrationBuilder.DropIndex(
                name: "IX_courses_TrainingTrackId",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "StartsAtUtc",
                table: "lectures");

            migrationBuilder.DropColumn(
                name: "DeliveryMode",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "ObjectivesAr",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "ObjectivesEn",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "PlannedEndUtc",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "PlannedHours",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "PlannedStartUtc",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "TargetAudienceAr",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "TargetAudienceEn",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "TrainingTrackId",
                table: "courses");
        }
    }
}
