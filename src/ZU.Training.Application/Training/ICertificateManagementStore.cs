using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Training;

public interface ICertificateManagementStore
{
    Task<Course?> CourseAsync(Guid id, CancellationToken ct);
    Task<CourseEnrollment?> EnrollmentAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<CourseEnrollment>> CourseEnrollmentsAsync(Guid courseId, CancellationToken ct);
    Task<UserAccount?> UserAsync(Guid id, CancellationToken ct);
    Task<Faculty?> FacultyAsync(Guid? id, CancellationToken ct);
    Task<CertificateTemplate?> LatestTemplateAsync(CancellationToken ct);
    Task<Certificate?> CertificateByEnrollmentIdAsync(Guid enrollmentId, CancellationToken ct);
    Task<Certificate?> CertificateByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<Certificate>> CertificatesByCourseIdAsync(Guid courseId, CancellationToken ct);
    Task<IReadOnlyList<Certificate>> CertificatesByUserIdAsync(Guid userId, CancellationToken ct);
    Task<bool> ReferenceExistsAsync(string reference, CancellationToken ct);
    void AddCertificate(Certificate certificate);
    Task SaveAsync(CancellationToken ct);
}
