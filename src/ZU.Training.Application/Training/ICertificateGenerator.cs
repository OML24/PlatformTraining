namespace ZU.Training.Application.Training;

public sealed record CertificateRenderModel(
    string RecipientNameAr,
    string? RecipientNameEn,
    string? EmployeeId,
    string CourseTitleAr,
    string? CourseTitleEn,
    string ActualInstructorNameAr,
    string? ActualInstructorNameEn,
    decimal PlannedHours,
    DateTimeOffset PlannedStartUtc,
    DateTimeOffset PlannedEndUtc,
    DateTimeOffset IssuedAtUtc,
    string ReferenceNumber,
    string VerificationCode,
    string VerificationUrl
);

public interface ICertificateGenerator
{
    byte[] GeneratePdf(CertificateRenderModel model);
}
