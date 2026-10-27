using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Training;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfCertificateVerificationStore(TrainingDbContext db) : ICertificateVerificationStore
{
    public Task<Certificate?> ByReferenceAsync(string reference, CancellationToken ct)
        => db.Certificates.AsNoTracking().SingleOrDefaultAsync(x => x.ReferenceNumber == reference, ct);
    public Task<CourseEnrollment?> EnrollmentAsync(Guid id, CancellationToken ct)
        => db.CourseEnrollments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
    public Task<Course?> CourseAsync(Guid id, CancellationToken ct)
        => db.Courses.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
    public Task<UserAccount?> UserAsync(Guid id, CancellationToken ct)
        => db.UserAccounts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, ct);
}
