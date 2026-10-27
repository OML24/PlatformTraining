using System.Security.Cryptography;
using ZU.Training.Application.Security;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Training;

public sealed record UserCertificateDto(
    Guid Id,
    Guid CourseId,
    string CourseTitleAr,
    string? CourseTitleEn,
    string ReferenceNumber,
    string VerificationCode,
    CertificateStatus Status,
    string? StatusReason,
    DateTimeOffset IssuedAtUtc,
    string ActualInstructorNameAr,
    string? ActualInstructorNameEn,
    decimal PlannedHours,
    string? FacultyNameAr,
    bool CanDownload
);

public sealed class CertificateService(
    ICertificateManagementStore store,
    CertificateGateService gateService,
    ICertificateGenerator generator,
    ICertificateStorage storage,
    IPermissionAccess permissions)
{
    private static readonly Guid FallbackTemplateId = Guid.Parse("11111111-2222-3333-4444-555555555555");

    public async Task<Certificate?> IssueOrReevaluateCertificateAsync(
        Guid courseId, Guid enrollmentId, string verificationBaseUrl, CancellationToken ct)
    {
        var course = await store.CourseAsync(courseId, ct);
        if (course is null || !course.IsActive) return null;

        var enrollment = await store.EnrollmentAsync(enrollmentId, ct);
        if (enrollment is null || !enrollment.IsActive) return null;

        var gateStatus = await gateService.StatusForEnrollmentAsync(course, enrollment, ct);
        var existingCert = await store.CertificateByEnrollmentIdAsync(enrollmentId, ct);

        if (gateStatus.IsEligible)
        {
            if (existingCert is not null)
            {
                if (existingCert.Status != CertificateStatus.Valid)
                {
                    existingCert.Status = CertificateStatus.Valid;
                    existingCert.StatusReason = null;
                    await store.SaveAsync(ct);
                }
                return existingCert;
            }

            var user = await store.UserAsync(enrollment.UserId, ct);
            if (user is null) return null;

            var faculty = await store.FacultyAsync(course.FacultyId, ct);
            var facultyCode = faculty?.ExternalCode?.Trim() ?? "GEN";

            var reference = await GenerateReferenceNumberAsync(facultyCode, ct);
            var verificationCode = Convert.ToHexStringLower(RandomNumberGenerator.GetBytes(8));
            var verifyUrl = $"{verificationBaseUrl.TrimEnd('/')}/verify?cert={reference}";

            var template = await store.LatestTemplateAsync(ct);
            var templateId = template?.Id ?? FallbackTemplateId;

            var renderModel = new CertificateRenderModel(
                RecipientNameAr: user.NameAr,
                RecipientNameEn: user.NameEn,
                EmployeeId: user.EmployeeId?.ToString() ?? user.LoginName,
                CourseTitleAr: course.TitleAr,
                CourseTitleEn: course.TitleEn,
                ActualInstructorNameAr: course.ActualInstructorNameAr,
                ActualInstructorNameEn: course.ActualInstructorNameEn,
                PlannedHours: course.PlannedHours,
                PlannedStartUtc: course.PlannedStartUtc,
                PlannedEndUtc: course.PlannedEndUtc,
                IssuedAtUtc: DateTimeOffset.UtcNow,
                ReferenceNumber: reference,
                VerificationCode: verificationCode,
                VerificationUrl: verifyUrl
            );

            var pdfBytes = generator.GeneratePdf(renderModel);
            var storageKey = await storage.SavePdfAsync(pdfBytes, ct);

            var certificate = new Certificate
            {
                CourseId = course.Id,
                EnrollmentId = enrollment.Id,
                TemplateId = templateId,
                ReferenceNumber = reference,
                VerificationCode = verificationCode,
                PdfStorageKey = storageKey,
                Status = CertificateStatus.Valid,
                IssuedAtUtc = DateTimeOffset.UtcNow
            };

            store.AddCertificate(certificate);
            await store.SaveAsync(ct);
            return certificate;
        }
        else
        {
            if (existingCert is not null && existingCert.Status == CertificateStatus.Valid)
            {
                existingCert.Status = CertificateStatus.Revoked;
                existingCert.StatusReason = string.Join("; ", gateStatus.Reasons);
                await store.SaveAsync(ct);
            }
            return existingCert;
        }
    }

    public async Task ReevaluateCourseCertificatesAsync(
        Guid courseId, string verificationBaseUrl, CancellationToken ct)
    {
        var enrollments = await store.CourseEnrollmentsAsync(courseId, ct);
        foreach (var enrollment in enrollments.Where(x => x.IsActive))
        {
            await IssueOrReevaluateCertificateAsync(courseId, enrollment.Id, verificationBaseUrl, ct);
        }
    }

    public async Task<IReadOnlyList<UserCertificateDto>> GetMyCertificatesAsync(
        Guid userId, CancellationToken ct)
    {
        var certs = await store.CertificatesByUserIdAsync(userId, ct);
        var result = new List<UserCertificateDto>();

        foreach (var cert in certs.Where(x => x.IsActive && x.Status != CertificateStatus.Hidden))
        {
            var course = await store.CourseAsync(cert.CourseId, ct);
            if (course is null) continue;
            var faculty = await store.FacultyAsync(course.FacultyId, ct);

            result.Add(new UserCertificateDto(
                cert.Id,
                course.Id,
                course.TitleAr,
                course.TitleEn,
                cert.ReferenceNumber,
                cert.VerificationCode,
                cert.Status,
                cert.StatusReason,
                cert.IssuedAtUtc,
                course.ActualInstructorNameAr,
                course.ActualInstructorNameEn,
                course.PlannedHours,
                faculty?.NameAr,
                CanDownload: cert.Status == CertificateStatus.Valid
            ));
        }

        return result;
    }

    public async Task<IReadOnlyList<UserCertificateDto>> GetUserPublicCertificatesAsync(
        Guid requesterId, Guid targetUserId, CancellationToken ct)
    {
        var certs = await store.CertificatesByUserIdAsync(targetUserId, ct);
        var result = new List<UserCertificateDto>();
        var isOwner = requesterId == targetUserId;
        var isAdmin = await permissions.IsAllowedAsync(requesterId, PermissionKeys.ManageAllCourses, ct);

        foreach (var cert in certs.Where(x => x.IsActive && x.Status == CertificateStatus.Valid))
        {
            var course = await store.CourseAsync(cert.CourseId, ct);
            if (course is null) continue;
            var faculty = await store.FacultyAsync(course.FacultyId, ct);

            result.Add(new UserCertificateDto(
                cert.Id,
                course.Id,
                course.TitleAr,
                course.TitleEn,
                cert.ReferenceNumber,
                cert.VerificationCode,
                cert.Status,
                cert.StatusReason,
                cert.IssuedAtUtc,
                course.ActualInstructorNameAr,
                course.ActualInstructorNameEn,
                course.PlannedHours,
                faculty?.NameAr,
                CanDownload: isOwner || isAdmin
            ));
        }

        return result;
    }

    public async Task<(Stream Stream, string FileName)> DownloadAsync(
        Guid certificateId, Guid requesterId, CancellationToken ct)
    {
        var cert = await store.CertificateByIdAsync(certificateId, ct)
            ?? throw new KeyNotFoundException("Certificate not found.");

        if (cert.Status != CertificateStatus.Valid)
            throw new InvalidOperationException("Certificate is not valid for download.");

        var enrollment = await store.EnrollmentAsync(cert.EnrollmentId, ct)
            ?? throw new KeyNotFoundException("Enrollment not found.");

        var isOwner = enrollment.UserId == requesterId;
        var isAdmin = await permissions.IsAllowedAsync(requesterId, PermissionKeys.ManageAllCourses, ct) ||
                      await permissions.IsAllowedAsync(requesterId, PermissionKeys.ApproveCourses, ct);

        if (!isOwner && !isAdmin)
            throw new UnauthorizedAccessException("You are not authorized to download this certificate.");

        var stream = await storage.OpenReadAsync(cert.PdfStorageKey, ct)
            ?? throw new FileNotFoundException("Certificate PDF file is missing.");

        var fileName = $"Certificate-{cert.ReferenceNumber}.pdf";
        return (stream, fileName);
    }

    private async Task<string> GenerateReferenceNumberAsync(string facultyCode, CancellationToken ct)
    {
        var year = DateTimeOffset.UtcNow.Year;
        for (var attempt = 0; attempt < 100; attempt++)
        {
            var seq = RandomNumberGenerator.GetInt32(100, 9999).ToString("D4");
            var candidate = $"ZU-{year}-{facultyCode}-{seq}";
            if (!await store.ReferenceExistsAsync(candidate, ct))
                return candidate;
        }
        return $"ZU-{year}-{facultyCode}-{Guid.NewGuid():N}"[..25];
    }
}
