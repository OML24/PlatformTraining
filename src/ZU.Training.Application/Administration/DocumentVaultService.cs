using ZU.Training.Application.Security;
using ZU.Training.Application.Training;
using ZU.Training.Domain.Identity;

namespace ZU.Training.Application.Administration;

public sealed class DocumentVaultService(
    IDocumentVaultStore store,
    IOfficialDocumentStorage storage,
    IPermissionAccess permissions)
{
    public async Task<IReadOnlyList<DocumentVaultItemDto>> ListDocumentsAsync(Guid actorId, CancellationToken ct)
    {
        var canView = await permissions.IsAllowedAsync(actorId, PermissionKeys.ViewOfficialDocuments, ct) ||
                      await permissions.IsAllowedAsync(actorId, PermissionKeys.ManageAllCourses, ct);
        if (!canView) throw new UnauthorizedAccessException();

        var docs = await store.ActiveDocumentsAsync(ct);
        var uploaderIds = docs.Select(x => x.UploadedByUserId).Distinct().ToList();
        var uploaders = await store.UserNamesAsync(uploaderIds, ct);

        return docs.Select(d => new DocumentVaultItemDto(
            d.Id,
            d.EntityType,
            d.EntityId,
            d.Kind,
            d.OriginalName,
            d.CreatedAtUtc,
            uploaders.GetValueOrDefault(d.UploadedByUserId)
        )).ToList();
    }

    public async Task<(Stream Stream, string FileName)> DownloadDocumentAsync(Guid actorId, Guid documentId, CancellationToken ct)
    {
        var canView = await permissions.IsAllowedAsync(actorId, PermissionKeys.ViewOfficialDocuments, ct) ||
                      await permissions.IsAllowedAsync(actorId, PermissionKeys.ManageAllCourses, ct);
        if (!canView) throw new UnauthorizedAccessException();

        var doc = await store.DocumentAsync(documentId, ct)
            ?? throw new KeyNotFoundException("Document not found.");

        var stream = await storage.OpenReadAsync(doc.StorageKey, ct)
            ?? throw new FileNotFoundException("Document file not found on disk.");

        return (stream, doc.OriginalName);
    }

    public async Task<IReadOnlyList<AuditLogItemDto>> ListAuditLogsAsync(Guid actorId, int take, CancellationToken ct)
    {
        var isAdmin = await permissions.IsAllowedAsync(actorId, PermissionKeys.ManageAllCourses, ct) ||
                      await permissions.IsAllowedAsync(actorId, PermissionKeys.ManageUsers, ct);
        if (!isAdmin) throw new UnauthorizedAccessException();

        take = Math.Clamp(take, 10, 500);
        var events = await store.RecentAuditEventsAsync(take, ct);
        var actorIds = events.Where(x => x.ActorUserId.HasValue).Select(x => x.ActorUserId!.Value).Distinct().ToList();
        var actors = await store.UserNamesAsync(actorIds, ct);

        return events.Select(e => new AuditLogItemDto(
            e.Id,
            e.CreatedAtUtc,
            e.ActorUserId.HasValue ? actors.GetValueOrDefault(e.ActorUserId.Value) : "النظام",
            e.ActionKey,
            e.EntityType,
            e.EntityId,
            e.IpAddress,
            e.Reason
        )).ToList();
    }
}
