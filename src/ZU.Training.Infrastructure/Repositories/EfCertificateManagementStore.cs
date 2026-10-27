using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Training;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfCertificateManagementStore(TrainingDbContext db) : ICertificateManagementStore
{
    public Task<Course?> CourseAsync(Guid id, CancellationToken ct)
        => db.Courses.SingleOrDefaultAsync(x => x.Id == id, ct);

    public Task<CourseEnrollment?> EnrollmentAsync(Guid id, CancellationToken ct)
        => db.CourseEnrollments.SingleOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<CourseEnrollment>> CourseEnrollmentsAsync(Guid courseId, CancellationToken ct)
        => await db.CourseEnrollments.Where(x => x.CourseId == courseId && x.IsActive).ToListAsync(ct);

    public Task<UserAccount?> UserAsync(Guid id, CancellationToken ct)
        => db.UserAccounts.SingleOrDefaultAsync(x => x.Id == id, ct);

    public Task<Faculty?> FacultyAsync(Guid? id, CancellationToken ct)
        => id is null ? Task.FromResult<Faculty?>(null) : db.Faculties.SingleOrDefaultAsync(x => x.Id == id, ct);

    public Task<CertificateTemplate?> LatestTemplateAsync(CancellationToken ct)
        => db.CertificateTemplates.OrderByDescending(x => x.Version).FirstOrDefaultAsync(ct);

    public Task<Certificate?> CertificateByEnrollmentIdAsync(Guid enrollmentId, CancellationToken ct)
        => db.Certificates.SingleOrDefaultAsync(x => x.EnrollmentId == enrollmentId && x.IsActive, ct);

    public Task<Certificate?> CertificateByIdAsync(Guid id, CancellationToken ct)
        => db.Certificates.SingleOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyList<Certificate>> CertificatesByCourseIdAsync(Guid courseId, CancellationToken ct)
        => await db.Certificates.Where(x => x.CourseId == courseId && x.IsActive).ToListAsync(ct);

    public async Task<IReadOnlyList<Certificate>> CertificatesByUserIdAsync(Guid userId, CancellationToken ct)
    {
        var enrollmentIds = await db.CourseEnrollments
            .Where(x => x.UserId == userId && x.IsActive)
            .Select(x => x.Id)
            .ToListAsync(ct);

        return await db.Certificates
            .Where(x => enrollmentIds.Contains(x.EnrollmentId) && x.IsActive)
            .OrderByDescending(x => x.IssuedAtUtc)
            .ToListAsync(ct);
    }

    public Task<bool> ReferenceExistsAsync(string reference, CancellationToken ct)
        => db.Certificates.AnyAsync(x => x.ReferenceNumber == reference, ct);

    public void AddCertificate(Certificate certificate) => db.Certificates.Add(certificate);

    public Task SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
