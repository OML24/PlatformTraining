using System.Text;
using ZU.Training.Application.Training;
using ZU.Training.Infrastructure.Storage;

namespace ZU.Training.Domain.Tests;

public sealed class CertificateGeneratorTests
{
    [Fact]
    public void GeneratePdf_ProducesValidPdfWithQrCode()
    {
        var generator = new CertificateGenerator();
        var model = new CertificateRenderModel(
            RecipientNameAr: "د. عمر إبراهيم المدفع",
            RecipientNameEn: "Dr. Omar Al-Madfa",
            EmployeeId: "8303",
            CourseTitleAr: "تطوير استراتيجيات التعليم الرقمي والتقييم الأكاديمي",
            CourseTitleEn: "Digital Teaching Strategies & Academic Evaluation",
            ActualInstructorNameAr: "أ.د. محمد خالد العمري",
            ActualInstructorNameEn: "Prof. Mohammad Al-Omari",
            PlannedHours: 15.0m,
            PlannedStartUtc: new DateTimeOffset(2026, 10, 15, 9, 0, 0, TimeSpan.Zero),
            PlannedEndUtc: new DateTimeOffset(2026, 10, 20, 14, 0, 0, TimeSpan.Zero),
            IssuedAtUtc: DateTimeOffset.UtcNow,
            ReferenceNumber: "ZU-2026-IT-8303",
            VerificationCode: "A1B2C3D4E5F67890",
            VerificationUrl: "http://127.0.0.1:5099/?cert=ZU-2026-IT-8303"
        );

        var pdfBytes = generator.GeneratePdf(model);

        Assert.NotNull(pdfBytes);
        Assert.NotEmpty(pdfBytes);
        // Standard PDF magic number %PDF
        var header = Encoding.ASCII.GetString(pdfBytes.AsSpan(0, 4));
        Assert.Equal("%PDF", header);
        // Verify size is substantial (contains QuestPDF elements + QRCoder PNG)
        Assert.True(pdfBytes.Length > 1000);
    }
}
