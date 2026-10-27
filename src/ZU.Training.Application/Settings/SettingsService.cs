using ZU.Training.Domain.Settings;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Settings;

public sealed class SettingsService(ISettingsStore store, ICertificateTemplateStorage storage)
{
    public Task<IReadOnlyList<FieldHint>> GetHintsAsync(CancellationToken cancellationToken)
        => store.GetHintsAsync(cancellationToken);

    public async Task<FieldHint> SaveHintAsync(Guid? id, FieldHintInput input, CancellationToken cancellationToken)
    {
        var screen = input.ScreenKey.Trim();
        var field = input.FieldKey.Trim();
        var arabic = input.TextAr.Trim();
        if (screen.Length is < 1 or > 150 || field.Length is < 1 or > 150 || arabic.Length is < 1 or > 2000 ||
            input.TextEn?.Length > 2000)
            throw new ArgumentException("Invalid field hint.");
        var existing = await store.FindHintByKeyAsync(screen, field, cancellationToken);
        if (existing is not null && (id.HasValue ? existing.Id != id.Value : existing.IsActive))
            throw new InvalidOperationException("This screen and field already have a hint.");

        var hint = id.HasValue ? await store.FindHintAsync(id.Value, cancellationToken) : existing;
        if (id.HasValue && hint is null)
            throw new KeyNotFoundException("Hint not found.");
        if (hint is null)
        {
            hint = new FieldHint { ScreenKey = screen, FieldKey = field, TextAr = arabic };
            store.AddHint(hint);
        }
        hint.ScreenKey = screen;
        hint.FieldKey = field;
        hint.TextAr = arabic;
        hint.TextEn = string.IsNullOrWhiteSpace(input.TextEn) ? null : input.TextEn.Trim();
        hint.IsActive = true;
        await store.SaveAsync(cancellationToken);
        return hint;
    }

    public async Task<bool> DeleteHintAsync(Guid id, CancellationToken cancellationToken)
    {
        var hint = await store.FindHintAsync(id, cancellationToken);
        if (hint is null || !hint.IsActive) return false;
        hint.IsActive = false;
        await store.SaveAsync(cancellationToken);
        return true;
    }

    public Task<PasswordPolicy?> GetPasswordPolicyAsync(CancellationToken cancellationToken)
        => store.GetPasswordPolicyAsync(cancellationToken);

    public async Task<PasswordPolicy> UpdatePasswordPolicyAsync(PasswordPolicyInput input, CancellationToken cancellationToken)
    {
        if (input.MinimumLength is < 8 or > 128 || input.MaximumLength < input.MinimumLength || input.MaximumLength > 256)
            throw new ArgumentException("Invalid password policy lengths.");
        var policy = await store.GetPasswordPolicyAsync(cancellationToken)
            ?? throw new InvalidOperationException("Password policy is missing.");
        policy.MinimumLength = input.MinimumLength;
        policy.MaximumLength = input.MaximumLength;
        policy.AllowSpaces = input.AllowSpaces;
        policy.RequireMixedCharacterClasses = input.RequireMixedCharacterClasses;
        policy.BlockCommonPasswords = input.BlockCommonPasswords;
        policy.EffectiveAtUtc = DateTimeOffset.UtcNow;
        await store.SaveAsync(cancellationToken);
        return policy;
    }

    public Task<IReadOnlyList<CertificateTemplate>> GetTemplatesAsync(CancellationToken cancellationToken)
        => store.GetTemplatesAsync(cancellationToken);

    public async Task<CertificateTemplate> UploadTemplateAsync(string nameAr, string? nameEn, Stream content,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(nameAr) || nameAr.Length > 300 || nameEn?.Length > 300)
            throw new ArgumentException("Invalid template name.");
        var (key, hash) = await storage.SavePdfAsync(content, cancellationToken);
        var template = new CertificateTemplate
        {
            NameAr = nameAr.Trim(), NameEn = string.IsNullOrWhiteSpace(nameEn) ? null : nameEn.Trim(),
            PdfStorageKey = key, Sha256 = hash, Version = await store.NextTemplateVersionAsync(cancellationToken)
        };
        store.AddTemplate(template);
        await store.SaveAsync(cancellationToken);
        return template;
    }

    public Task<Stream?> OpenTemplateAsync(string key, CancellationToken cancellationToken)
        => storage.OpenReadAsync(key, cancellationToken);
}
