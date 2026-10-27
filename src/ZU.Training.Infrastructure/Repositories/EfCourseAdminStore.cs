using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Administration;
using ZU.Training.Domain.Training;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfCourseAdminStore(TrainingDbContext db) : ICourseAdminStore
{
    public async Task<IReadOnlyList<Course>> CoursesAsync(CancellationToken ct)
        => await db.Courses.OrderByDescending(x => x.CreatedAtUtc).ToListAsync(ct);
    public Task<Course?> CourseAsync(Guid id, CancellationToken ct)
        => db.Courses.SingleOrDefaultAsync(x => x.Id == id, ct);
    public async Task<IReadOnlyList<Certificate>> CertificatesAsync(Guid courseId, CancellationToken ct)
        => await db.Certificates.Where(x => x.CourseId == courseId && x.IsActive).ToListAsync(ct);
    public Task<CourseEnrollment?> EnrollmentAsync(Guid id, CancellationToken ct)
        => db.CourseEnrollments.SingleOrDefaultAsync(x => x.Id == id, ct);
    public Task SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
