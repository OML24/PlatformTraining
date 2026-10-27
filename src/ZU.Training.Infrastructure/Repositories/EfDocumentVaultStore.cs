using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Administration;
using ZU.Training.Domain.Settings;
using ZU.Training.Domain.Training;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfDocumentVaultStore(TrainingDbContext db) : IDocumentVaultStore
{
    public async Task<IReadOnlyList<OfficialDocument>> ActiveDocumentsAsync(CancellationToken ct)
        => await db.OfficialDocuments.AsNoTracking()
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(ct);

    public Task<OfficialDocument?> DocumentAsync(Guid id, CancellationToken ct)
        => db.OfficialDocuments.SingleOrDefaultAsync(x => x.Id == id && x.IsActive, ct);

    public async Task<IReadOnlyDictionary<Guid, string>> UserNamesAsync(IReadOnlyList<Guid> userIds, CancellationToken ct)
        => await db.UserAccounts.AsNoTracking()
            .Where(x => userIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x.NameAr, ct);

    public async Task<IReadOnlyList<AuditEvent>> RecentAuditEventsAsync(int take, CancellationToken ct)
        => await db.AuditEvents.AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(take)
            .ToListAsync(ct);
}
