using ZU.Training.Domain.Settings;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Settings;

public sealed record FieldHintInput(string ScreenKey, string FieldKey, string TextAr, string? TextEn);
public sealed record PasswordPolicyInput(int MinimumLength, int MaximumLength, bool AllowSpaces,
    bool RequireMixedCharacterClasses, bool BlockCommonPasswords);

public interface ISettingsStore
{
    Task<IReadOnlyList<FieldHint>> GetHintsAsync(CancellationToken cancellationToken);
    Task<FieldHint?> FindHintAsync(Guid id, CancellationToken cancellationToken);
    Task<FieldHint?> FindHintByKeyAsync(string screenKey, string fieldKey, CancellationToken cancellationToken);
    Task<bool> HintKeyExistsAsync(string screenKey, string fieldKey, Guid? exceptId, CancellationToken cancellationToken);
    Task<PasswordPolicy?> GetPasswordPolicyAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<CertificateTemplate>> GetTemplatesAsync(CancellationToken cancellationToken);
    Task<int> NextTemplateVersionAsync(CancellationToken cancellationToken);
    void AddHint(FieldHint hint);
    void AddTemplate(CertificateTemplate template);
    Task SaveAsync(CancellationToken cancellationToken);
}

public interface ICertificateTemplateStorage
{
    Task<(string StorageKey, string Sha256)> SavePdfAsync(Stream content, CancellationToken cancellationToken);
    Task<Stream?> OpenReadAsync(string storageKey, CancellationToken cancellationToken);
}
