using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Training;

public sealed record ImpactSummary(Guid Id, Guid EnrollmentId, int Version, string OriginalName,
    ImpactStatus Status, string? ReviewNoteAr, DateTimeOffset CreatedAtUtc);

public interface IImpactStore
{
    Task<UserAccount?> UserAsync(Guid id, CancellationToken ct);
    Task<Course?> CourseAsync(Guid id, CancellationToken ct);
    Task<CourseEnrollment?> EnrollmentAsync(Guid courseId, Guid userId, CancellationToken ct);
    Task<CourseEnrollment?> EnrollmentByIdAsync(Guid id, CancellationToken ct);
    Task<ImpactSubmission?> SubmissionAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<ImpactSubmission>> SubmissionsAsync(Guid enrollmentId, CancellationToken ct);
    Task<IReadOnlyList<CourseEnrollment>> EnrollmentsAsync(Guid courseId, CancellationToken ct);
    void Add(ImpactSubmission submission);
    Task SaveAsync(CancellationToken ct);
}
