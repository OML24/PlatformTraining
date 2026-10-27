using ZU.Training.Application.Security;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Training;

public sealed class CertificateGateService(ICertificateGateStore store, IPermissionAccess permissions)
{
    public async Task<CertificateGateStatus> StatusAsync(Guid userId, Guid courseId, CancellationToken ct)
    {
        if (!await permissions.IsAllowedAsync(userId, PermissionKeys.ViewCourses, ct))
            throw new UnauthorizedAccessException();
        var course = await store.CourseAsync(courseId, ct) ?? throw new KeyNotFoundException("Course not found.");
        var enrollment = await store.EnrollmentAsync(courseId, userId, ct);
        if (enrollment?.IsActive != true) throw new UnauthorizedAccessException();
        return await StatusForEnrollmentAsync(course, enrollment, ct);
    }

    public async Task<CertificateGateStatus> StatusForEnrollmentAsync(Course course,
        CourseEnrollment enrollment, CancellationToken ct)
    {
        var total = await store.LectureCountAsync(course.Id, ct);
        var attended = await store.AttendanceCountAsync(course.Id, enrollment.Id, ct);
        var exam = await store.ExamOutcomeAsync(course.Id, enrollment.Id, ct);
        var evaluation = await store.HasEvaluationAsync(course.EvaluationFormVersionId, enrollment.Id, ct);
        var impact = await store.HasApprovedImpactAsync(enrollment.Id, ct);
        var result = CertificateEligibility.Evaluate(new CertificateEligibilityInput(
            course.Status == CourseStatus.Ended, total, attended, course.HasExam, exam, evaluation, impact));
        var certificate = await store.CertificateAsync(enrollment.Id, ct);
        return new CertificateGateStatus(course.Id, total, attended, exam, evaluation, impact,
            result.IsEligible, result.Reasons, certificate?.Status);
    }
}
