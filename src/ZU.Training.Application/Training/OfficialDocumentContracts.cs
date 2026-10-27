namespace ZU.Training.Application.Training;

public sealed record StoredOfficialDocument(string StorageKey, string OriginalName, string Sha256);

public interface IOfficialDocumentStorage
{
    Task<StoredOfficialDocument> SaveAsync(Stream content, string originalName,
        CancellationToken cancellationToken);
    Task<Stream?> OpenReadAsync(string storageKey, CancellationToken cancellationToken);
    Task DeleteAsync(string storageKey, CancellationToken cancellationToken);
}
