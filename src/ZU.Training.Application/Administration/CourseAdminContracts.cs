using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Administration;

public sealed record AdminCourseSummary(Guid Id, string TitleAr, string? TitleEn,
    CourseStatus Status, bool IsActive, bool IsArchived, int CertificateCount);

public interface ICourseAdminStore
{
    Task<IReadOnlyList<Course>> CoursesAsync(CancellationToken ct);
    Task<Course?> CourseAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Certificate>> CertificatesAsync(Guid courseId, CancellationToken ct);
    Task<CourseEnrollment?> EnrollmentAsync(Guid id, CancellationToken ct);
    Task SaveAsync(CancellationToken ct);
}
