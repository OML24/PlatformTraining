using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Settings;
using ZU.Training.Domain.Settings;
using ZU.Training.Domain.Training;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfSettingsStore(TrainingDbContext db) : ISettingsStore
{
    public async Task<IReadOnlyList<FieldHint>> GetHintsAsync(CancellationToken cancellationToken)
        => await db.FieldHints.AsNoTracking().Where(x => x.IsActive)
            .OrderBy(x => x.ScreenKey).ThenBy(x => x.FieldKey).ToListAsync(cancellationToken);

    public Task<FieldHint?> FindHintAsync(Guid id, CancellationToken cancellationToken)
        => db.FieldHints.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<FieldHint?> FindHintByKeyAsync(string screenKey, string fieldKey, CancellationToken cancellationToken)
        => db.FieldHints.SingleOrDefaultAsync(x => x.ScreenKey == screenKey && x.FieldKey == fieldKey,
            cancellationToken);

    public Task<bool> HintKeyExistsAsync(string screenKey, string fieldKey, Guid? exceptId,
        CancellationToken cancellationToken)
        => db.FieldHints.AnyAsync(x => x.ScreenKey == screenKey && x.FieldKey == fieldKey &&
            (!exceptId.HasValue || x.Id != exceptId.Value), cancellationToken);

    public Task<PasswordPolicy?> GetPasswordPolicyAsync(CancellationToken cancellationToken)
        => db.PasswordPolicies.Where(x => x.IsActive).OrderByDescending(x => x.EffectiveAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<IReadOnlyList<CertificateTemplate>> GetTemplatesAsync(CancellationToken cancellationToken)
        => await db.CertificateTemplates.AsNoTracking().Where(x => x.IsActive)
            .OrderByDescending(x => x.Version).ToListAsync(cancellationToken);

    public async Task<int> NextTemplateVersionAsync(CancellationToken cancellationToken)
        => 1 + (await db.CertificateTemplates.MaxAsync(x => (int?)x.Version, cancellationToken) ?? 0);

    public void AddHint(FieldHint hint) => db.FieldHints.Add(hint);
    public void AddTemplate(CertificateTemplate template) => db.CertificateTemplates.Add(template);
    public Task SaveAsync(CancellationToken cancellationToken) => db.SaveChangesAsync(cancellationToken);
}
