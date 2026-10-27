using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ZU.Training.Application.Training;

namespace ZU.Training.Infrastructure.Storage;

public sealed class CertificateGenerator : ICertificateGenerator
{
    static CertificateGenerator()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        QuestPDF.Settings.UseSystemFonts = true;
        QuestPDF.Settings.ThrowOnMissingTextGlyphs = false;
    }

    public byte[] GeneratePdf(CertificateRenderModel model)
    {
        byte[] qrBytes;
        using (var qrGenerator = new QRCodeGenerator())
        using (var qrCodeData = qrGenerator.CreateQrCode(model.VerificationUrl, QRCodeGenerator.ECCLevel.Q))
        using (var qrCode = new PngByteQRCode(qrCodeData))
        {
            qrBytes = qrCode.GetGraphic(12);
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(24);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontFamily("Arial", "Tahoma", "Segoe UI"));

                page.Content().Border(3).BorderColor("#022C5A").Padding(6).Border(1.5f).BorderColor("#C5A059").Padding(16).Column(col =>
                {
                    // Header
                    col.Item().Row(row =>
                    {
                        row.RelativeItem().Column(c =>
                        {
                            c.Item().Text("المملكة الأردنية الهاشمية").FontSize(11).Bold().FontColor("#022C5A");
                            c.Item().Text("جامعة الزرقاء").FontSize(16).ExtraBold().FontColor("#022C5A");
                            c.Item().Text("عمادة التطوير الأكاديمي وضمان الجودة").FontSize(10).FontColor("#475569");
                        });

                        row.RelativeItem().AlignRight().Column(c =>
                        {
                            c.Item().Text("The Hashemite Kingdom of Jordan").FontSize(9).FontColor("#64748B");
                            c.Item().Text("ZARQA UNIVERSITY").FontSize(14).ExtraBold().FontColor("#022C5A");
                            c.Item().Text("Deanship of Academic Development & Quality").FontSize(9).FontColor("#475569");
                        });
                    });

                    col.Item().PaddingTop(6).LineHorizontal(1.5f).LineColor("#C5A059");

                    // Title
                    col.Item().PaddingTop(12).AlignCenter().Column(c =>
                    {
                        c.Item().Text("شهادة إتمام برنامج تدريبي").FontSize(22).ExtraBold().FontColor("#022C5A");
                        c.Item().Text("CERTIFICATE OF TRAINING COMPLETION").FontSize(10).Bold().LetterSpacing(0.1f).FontColor("#C5A059");
                    });

                    // Body
                    col.Item().PaddingTop(12).AlignCenter().Column(c =>
                    {
                        c.Item().Text("تشهد عمادة التطوير الأكاديمي وضمان الجودة في جامعة الزرقاء بأن الزميل / الزميلة:")
                            .FontSize(11).FontColor("#334155");

                        var recipientText = model.RecipientNameAr;
                        if (!string.IsNullOrWhiteSpace(model.RecipientNameEn))
                            recipientText += $" / {model.RecipientNameEn}";
                        c.Item().PaddingVertical(4).Text(recipientText).FontSize(18).ExtraBold().FontColor("#022C5A");

                        if (!string.IsNullOrWhiteSpace(model.EmployeeId))
                        {
                            c.Item().Text($"الرقم الوظيفي: {model.EmployeeId}").FontSize(9).FontColor("#64748B");
                        }

                        c.Item().PaddingTop(8).Text("قد اجتاز(ت) بنجاح متطلبات البرنامج التدريبي المهني بعنوان:")
                            .FontSize(11).FontColor("#334155");

                        var courseText = model.CourseTitleAr;
                        if (!string.IsNullOrWhiteSpace(model.CourseTitleEn))
                            courseText += $" - {model.CourseTitleEn}";
                        c.Item().PaddingVertical(4).Text(courseText).FontSize(15).Bold().FontColor("#0F172A");

                        var hoursStr = model.PlannedHours.ToString("0.#");
                        var startStr = model.PlannedStartUtc.ToString("yyyy/MM/dd");
                        var endStr = model.PlannedEndUtc.ToString("yyyy/MM/dd");
                        c.Item().Text($"بواقع ({hoursStr}) ساعة تدريبية معتمدة، المنعقد في الفترة من {startStr} إلى {endStr}")
                            .FontSize(10.5f).FontColor("#475569");

                        c.Item().PaddingTop(2).Text($"المدرب المعتمد: {model.ActualInstructorNameAr}")
                            .FontSize(10.5f).Bold().FontColor("#022C5A");
                    });

                    col.Item().PaddingTop(10).LineHorizontal(1).LineColor("#E2E8F0");

                    // Footer / Verification and Signatures
                    col.Item().PaddingTop(8).Row(row =>
                    {
                        // QR & Verification block
                        row.RelativeItem(3).Row(qrRow =>
                        {
                            qrRow.ConstantItem(68).Image(qrBytes);
                            qrRow.RelativeItem().PaddingLeft(8).Column(c =>
                            {
                                c.Item().Text("التحقق الرقمي الرسمي").FontSize(9).Bold().FontColor("#022C5A");
                                c.Item().Text($"الرقم المرجعي: {model.ReferenceNumber}").FontSize(8.5f).FontColor("#0F172A");
                                c.Item().Text($"تاريخ الإصدار: {model.IssuedAtUtc:yyyy/MM/dd}").FontSize(8).FontColor("#64748B");
                                c.Item().Text("امسح الرمز للتحقق من صحة الشهادة").FontSize(7.5f).Italic().FontColor("#94A3B8");
                            });
                        });

                        // Official Signatures
                        row.RelativeItem(3).AlignRight().Row(sigRow =>
                        {
                            sigRow.RelativeItem().AlignCenter().Column(c =>
                            {
                                c.Item().Text("عميد التطوير وضمان الجودة").FontSize(9).Bold().FontColor("#022C5A");
                                c.Item().PaddingTop(22).Text("Deanship of Quality Assurance").FontSize(7.5f).FontColor("#64748B");
                            });

                            sigRow.RelativeItem().AlignCenter().Column(c =>
                            {
                                c.Item().Text("رئيس جامعة الزرقاء").FontSize(9).Bold().FontColor("#022C5A");
                                c.Item().PaddingTop(22).Text("President of Zarqa University").FontSize(7.5f).FontColor("#64748B");
                            });
                        });
                    });
                });
            });
        });

        return document.GeneratePdf();
    }
}
