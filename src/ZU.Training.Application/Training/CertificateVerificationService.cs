using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Training;

public sealed class CertificateVerificationService(ICertificateVerificationStore store)
{
    public async Task<CertificateVerification> VerifyAsync(string reference, CancellationToken ct)
    {
        reference = reference.Trim();
        if (reference.Length is < 6 or > 100) throw new ArgumentException("Invalid certificate reference.");
        var certificate = await store.ByReferenceAsync(reference, ct);
        if (certificate is null || !certificate.IsActive || certificate.Status == CertificateStatus.Hidden)
            throw new KeyNotFoundException("Certificate not found.");
        var enrollment = await store.EnrollmentAsync(certificate.EnrollmentId, ct)
            ?? throw new KeyNotFoundException("Certificate enrollment not found.");
        var recipient = await store.UserAsync(enrollment.UserId, ct)
            ?? throw new KeyNotFoundException("Certificate recipient not found.");
        var course = await store.CourseAsync(certificate.CourseId, ct)
            ?? throw new KeyNotFoundException("Certificate course not found.");
        return new CertificateVerification(certificate.ReferenceNumber, certificate.Status,
            certificate.StatusReason, recipient.NameAr, recipient.NameEn,
            course.TitleAr, course.TitleEn, course.ActualInstructorNameAr,
            course.ActualInstructorNameEn, certificate.IssuedAtUtc);
    }
}
