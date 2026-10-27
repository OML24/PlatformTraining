using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ZU.Training.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedFieldHints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "field_hints",
                columns: new[] { "Id", "CreatedAtUtc", "FieldKey", "ScreenKey", "TextAr", "TextEn", "UpdatedAtUtc" },
                values: new object[,]
                {
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940001"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "loginName", "auth", "أدخل الرقم الوظيفي المعتمد للدخول.", "Enter your employee number.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940002"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "password", "auth", "أدخل كلمة المرور الحالية أو الأولية في أول دخول.", "Enter your current or initial password.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940003"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "rememberMe", "auth", "يحفظ تسجيل الدخول لمدة أطول على هذا الجهاز.", "Keeps you signed in longer on this device.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940004"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "oldPassword", "auth", "أدخل كلمة المرور المستخدمة حاليًا.", "Enter the password currently in use.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940005"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "newPassword", "auth", "اختر كلمة جديدة توافق السياسة الحالية.", "Choose a password that meets the current policy.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940006"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "confirmPassword", "auth", "أعد كتابة كلمة المرور الجديدة كما هي.", "Repeat the new password exactly.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940007"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "hintScreen", "settings", "اسم الشاشة الذي يرتبط به التلميح، مثل auth أو course.", "Screen identifier, such as auth or course.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940008"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "hintField", "settings", "اسم الحقل داخل الشاشة المختارة.", "Field identifier within the screen.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940009"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "hintAr", "settings", "النص العربي الذي يظهر عند لمس الحقل.", "Arabic text shown when focusing the field.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594000a"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "hintEn", "settings", "ترجمة اختيارية؛ تظهر العربية عند تركها فارغة.", "Optional translation; Arabic is used if blank.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594000b"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "minimumLength", "settings", "أقل عدد أحرف لكلمة المرور الجديدة.", "Minimum length for new passwords.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594000c"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "maximumLength", "settings", "أكبر عدد أحرف لكلمة المرور الجديدة.", "Maximum length for new passwords.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594000d"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "allowSpaces", "settings", "حدد إذا كانت المسافات مسموحة في كلمة المرور.", "Allow spaces in passwords.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594000e"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "requireMix", "settings", "حدد إذا لزم وجود حروف وأرقام معًا.", "Require both letters and digits.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594000f"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "blockCommon", "settings", "يمنع كلمات المرور الأكثر شيوعًا.", "Blocks common passwords.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940010"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "templateNameAr", "settings", "اسم إداري للقالب باللغة العربية.", "Administrative template name in Arabic.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940011"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "templateNameEn", "settings", "الاسم الإنجليزي اختياري.", "English name is optional.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940012"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "templateFile", "settings", "ارفع ملف PDF ثابتًا لا يتجاوز 20 ميغابايت.", "Upload a fixed PDF of up to 20 MB.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940013"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "titleAr", "course", "اكتب الاسم الرسمي للدورة بالعربية.", "Enter the official Arabic course title.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940014"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "titleEn", "course", "الاسم الإنجليزي اختياري.", "English title is optional.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940015"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "trainingTrack", "course", "اختر محور التدريب الذي تتبعه الدورة.", "Choose the course training track.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940016"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "deliveryMode", "course", "حدد وجاهيًا أو عن بعد أو مدمجًا.", "Select in person, online, or hybrid.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940017"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "objectivesAr", "course", "اكتب أهداف الدورة ونواتج تعلمها بالعربية.", "Enter Arabic objectives and outcomes.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940018"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "objectivesEn", "course", "ترجمة الأهداف اختيارية.", "English objectives are optional.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940019"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "audienceAr", "course", "حدد الفئة المستهدفة بالعربية.", "Describe the target audience in Arabic.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594001a"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "audienceEn", "course", "ترجمة الفئة المستهدفة اختيارية.", "English audience is optional.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594001b"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "plannedHours", "course", "أدخل عدد الساعات المخطط لها.", "Enter the planned training hours.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594001c"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "plannedStart", "course", "أدخل التاريخ والوقت بصيغة سنة-شهر-يوم ساعة:دقيقة.", "Use YYYY-MM-DD HH:mm.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594001d"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "plannedEnd", "course", "يجب أن يكون بعد موعد البداية.", "Must be later than the start.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594001e"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "faculty", "course", "اختر كلية الدورة؛ اتركها فارغة للدورة العامة التي تنشئها الجودة.", "Choose a faculty or leave blank for a Quality general course.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a2127594001f"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "enrollmentMode", "course", "التسجيل الذاتي للمؤهلين أو اختيار الأسماء من الجودة.", "Self registration or Quality selection.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940020"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "actualInstructor", "course", "الاسم الذي يظهر للمتدربين وعلى الشهادة.", "Name shown to trainees and on certificates.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940021"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "instructorSearch", "course", "ابحث عن عضو الهيئة بالاسم أو الرقم الوظيفي.", "Search by employee name or number.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940022"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "qualityOperated", "course", "تدير الجودة دورة قصيرة باسم المحاضر الفعلي دون حساب له.", "Quality operates a short course for the named instructor.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940023"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "hasExam", "course", "إذا حددته، يلزم نجاح الامتحان للشهادة.", "Exam pass is required for certificates when selected.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) },
                    { new Guid("e13984f2-88ba-4891-92f9-a21275940024"), new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "evaluationDeadline", "course", "تحدد الجودة موعد إغلاق تقييم الدورة.", "Quality sets the evaluation closing date.", new DateTimeOffset(new DateTime(2026, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940001"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940002"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940003"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940004"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940005"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940006"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940007"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940008"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940009"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594000a"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594000b"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594000c"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594000d"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594000e"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594000f"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940010"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940011"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940012"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940013"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940014"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940015"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940016"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940017"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940018"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940019"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594001a"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594001b"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594001c"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594001d"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594001e"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a2127594001f"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940020"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940021"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940022"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940023"));

            migrationBuilder.DeleteData(
                table: "field_hints",
                keyColumn: "Id",
                keyValue: new Guid("e13984f2-88ba-4891-92f9-a21275940024"));
        }
    }
}
