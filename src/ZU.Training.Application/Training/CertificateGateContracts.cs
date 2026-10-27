using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Training;

public sealed record CertificateGateStatus(Guid CourseId, int TotalLectures, int AttendedLectures,
    ExamOutcome ExamOutcome, bool EvaluationCompleted, bool ImpactApproved,
    bool IsEligible, IReadOnlyList<CertificateIneligibilityReason> Reasons,
    CertificateStatus? ExistingCertificateStatus);

public interface ICertificateGateStore
{
    Task<Course?> CourseAsync(Guid id, CancellationToken ct);
    Task<CourseEnrollment?> EnrollmentAsync(Guid courseId, Guid userId, CancellationToken ct);
    Task<int> LectureCountAsync(Guid courseId, CancellationToken ct);
    Task<int> AttendanceCountAsync(Guid courseId, Guid enrollmentId, CancellationToken ct);
    Task<ExamOutcome> ExamOutcomeAsync(Guid courseId, Guid enrollmentId, CancellationToken ct);
    Task<bool> HasEvaluationAsync(Guid? versionId, Guid enrollmentId, CancellationToken ct);
    Task<bool> HasApprovedImpactAsync(Guid enrollmentId, CancellationToken ct);
    Task<Certificate?> CertificateAsync(Guid enrollmentId, CancellationToken ct);
}
