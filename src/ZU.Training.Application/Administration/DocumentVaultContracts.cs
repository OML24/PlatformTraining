using ZU.Training.Domain.Settings;
using ZU.Training.Domain.Training;

namespace ZU.Training.Application.Administration;

public sealed record DocumentVaultItemDto(
    Guid Id,
    string EntityType,
    Guid EntityId,
    string Kind,
    string OriginalName,
    DateTimeOffset CreatedAtUtc,
    string? UploaderNameAr
);

public sealed record AuditLogItemDto(
    Guid Id,
    DateTimeOffset TimestampUtc,
    string? ActorNameAr,
    string Action,
    string EntityType,
    Guid? EntityId,
    string? IpAddress,
    string? Details
);

public interface IDocumentVaultStore
{
    Task<IReadOnlyList<OfficialDocument>> ActiveDocumentsAsync(CancellationToken ct);
    Task<OfficialDocument?> DocumentAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyDictionary<Guid, string>> UserNamesAsync(IReadOnlyList<Guid> userIds, CancellationToken ct);
    Task<IReadOnlyList<AuditEvent>> RecentAuditEventsAsync(int take, CancellationToken ct);
}
