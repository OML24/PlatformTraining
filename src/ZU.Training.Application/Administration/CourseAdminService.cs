using ZU.Training.Application.Security;
using ZU.Training.Application.Training;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Administration;

public sealed class CourseAdminService(ICourseAdminStore store, CertificateGateService gate,
    IAuthStore auth, IPermissionAccess permissions)
{
    public async Task<IReadOnlyList<AdminCourseSummary>> ListAsync(Guid actorId, CancellationToken ct)
    {
        await RequireAdminAsync(actorId, ct);
        var rows = new List<AdminCourseSummary>();
        foreach (var course in await store.CoursesAsync(ct))
            rows.Add(new AdminCourseSummary(course.Id, course.TitleAr, course.TitleEn,
                course.Status, course.IsActive, course.IsArchived,
                (await store.CertificatesAsync(course.Id, ct)).Count));
        return rows;
    }

    public async Task<AdminCourseSummary> DeactivateAsync(Guid actorId, Guid courseId,
        bool hideCertificates, bool acknowledged, CancellationToken ct)
    {
        await RequireAdminAsync(actorId, ct);
        if (!acknowledged) throw new ArgumentException("The deletion warning must be acknowledged.");
        var course = await store.CourseAsync(courseId, ct) ?? throw new KeyNotFoundException("Course not found.");
        if (!course.IsActive) throw new InvalidOperationException("The course is already disabled.");
        var certificates = await store.CertificatesAsync(courseId, ct);
        course.IsActive = false;
        if (hideCertificates)
        {
            foreach (var certificate in certificates)
            {
                if (certificate.Status != CertificateStatus.Valid) continue;
                certificate.Status = CertificateStatus.Hidden;
                certificate.StatusReason = "AdminCourseDisabled";
            }
        }
        await store.SaveAsync(ct);
        return new AdminCourseSummary(course.Id, course.TitleAr, course.TitleEn,
            course.Status, false, course.IsArchived, certificates.Count);
    }

    public async Task<AdminCourseSummary> RestoreAsync(Guid actorId, Guid courseId, CancellationToken ct)
    {
        await RequireAdminAsync(actorId, ct);
        var course = await store.CourseAsync(courseId, ct) ?? throw new KeyNotFoundException("Course not found.");
        if (course.IsActive) throw new InvalidOperationException("The course is already active.");
        course.IsActive = true;
        var certificates = await store.CertificatesAsync(courseId, ct);
        foreach (var certificate in certificates.Where(x => x.Status == CertificateStatus.Hidden &&
            x.StatusReason == "AdminCourseDisabled"))
        {
            var enrollment = await store.EnrollmentAsync(certificate.EnrollmentId, ct);
            if (enrollment?.IsActive != true) continue;
            var status = await gate.StatusForEnrollmentAsync(course, enrollment, ct);
            if (!status.IsEligible) continue;
            certificate.Status = CertificateStatus.Valid;
            certificate.StatusReason = null;
        }
        await store.SaveAsync(ct);
        return new AdminCourseSummary(course.Id, course.TitleAr, course.TitleEn,
            course.Status, true, course.IsArchived, certificates.Count);
    }

    private async Task RequireAdminAsync(Guid actorId, CancellationToken ct)
    {
        if (!await permissions.IsAllowedAsync(actorId, PermissionKeys.ManageAllCourses, ct) ||
            !(await auth.GetRoleKeysAsync(actorId, ct)).Contains("ADMIN", StringComparer.Ordinal))
            throw new UnauthorizedAccessException();
    }
}
