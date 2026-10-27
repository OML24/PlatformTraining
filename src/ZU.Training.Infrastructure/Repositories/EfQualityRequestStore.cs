using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Training;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Training;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfQualityRequestStore(TrainingDbContext db) : IQualityRequestStore
{
    public Task<UserAccount?> UserAsync(Guid id, CancellationToken ct)
        => db.UserAccounts.SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);
    public Task<bool> FacultyExistsAsync(Guid id, CancellationToken ct)
        => db.Faculties.AnyAsync(x => x.Id == id && x.IsActive, ct);
    public Task<Course?> CourseAsync(Guid id, CancellationToken ct)
        => db.Courses.SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);
    public Task<TrainingNeed?> NeedAsync(Guid id, CancellationToken ct)
        => db.TrainingNeeds.SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);
    public async Task<IReadOnlyList<TrainingNeed>> NeedsAsync(CancellationToken ct)
        => await db.TrainingNeeds.Where(x => x.IsActive).OrderByDescending(x => x.CreatedAtUtc).ToListAsync(ct);
    public Task<CourseCancellationRequest?> CancellationAsync(Guid id, CancellationToken ct)
        => db.CourseCancellationRequests.SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);
    public async Task<IReadOnlyList<CourseCancellationRequest>> CancellationsAsync(CancellationToken ct)
        => await db.CourseCancellationRequests.Where(x => x.IsActive).OrderByDescending(x => x.CreatedAtUtc).ToListAsync(ct);
    public async Task<IReadOnlyList<Certificate>> CertificatesAsync(Guid courseId, CancellationToken ct)
        => await db.Certificates.Where(x => x.CourseId == courseId && x.IsActive).ToListAsync(ct);
    public Task<OfficialDocument?> DocumentAsync(Guid id, CancellationToken ct)
        => db.OfficialDocuments.SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);
    public async Task<IReadOnlyList<OfficialDocument>> DocumentsAsync(string entityType, Guid entityId, CancellationToken ct)
        => await db.OfficialDocuments.AsNoTracking().Where(x => x.EntityType == entityType &&
            x.EntityId == entityId && x.IsActive).OrderBy(x => x.CreatedAtUtc).ToListAsync(ct);
    public void AddNeed(TrainingNeed x) => db.TrainingNeeds.Add(x);
    public void AddCancellation(CourseCancellationRequest x) => db.CourseCancellationRequests.Add(x);
    public void AddDocument(OfficialDocument x) => db.OfficialDocuments.Add(x);
    public Task SaveAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
