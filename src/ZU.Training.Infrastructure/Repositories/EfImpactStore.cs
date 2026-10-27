using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Training;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfImpactStore(TrainingDbContext db) : IImpactStore
{
    public Task<UserAccount?> UserAsync(Guid id, CancellationToken ct)
        => db.UserAccounts.SingleOrDefaultAsync(x => x.Id == id, ct);
    public Task<Course?> CourseAsync(Guid id, CancellationToken ct)
        => db.Courses.SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);
    public Task<CourseEnrollment?> EnrollmentAsync(Guid courseId, Guid userId, CancellationToken ct)
        => db.CourseEnrollments.SingleOrDefaultAsync(x => x.CourseId == courseId && x.UserId == userId, ct);
    public Task<CourseEnrollment?> EnrollmentByIdAsync(Guid id, CancellationToken ct)
        => db.CourseEnrollments.SingleOrDefaultAsync(x => x.Id == id, ct);
    public Task<ImpactSubmission?> SubmissionAsync(Guid id, CancellationToken ct)
        => db.ImpactSubmissions.SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);
    public async Task<IReadOnlyList<ImpactSubmission>> SubmissionsAsync(Guid enrollmentId, CancellationToken ct)
        => await db.ImpactSubmissions.Where(x => x.EnrollmentId == enrollmentId && x.IsActive)
            .OrderBy(x => x.Version).ToListAsync(ct);
    public async Task<IReadOnlyList<CourseEnrollment>> EnrollmentsAsync(Guid courseId, CancellationToken ct)
        => await db.CourseEnrollments.Where(x => x.CourseId == courseId && x.IsActive).ToListAsync(ct);
    public void Add(ImpactSubmission submission) => db.ImpactSubmissions.Add(submission);
    public Task SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
