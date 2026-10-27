using ZU.Training.Application.Training;

namespace ZU.Training.Infrastructure.Storage;

public sealed class CertificateStorage(string root) : ICertificateStorage
{
    public async Task<string> SavePdfAsync(byte[] bytes, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(root);
        var key = $"{Guid.NewGuid():N}.pdf";
        var path = Path.Combine(root, key);
        await File.WriteAllBytesAsync(path, bytes, cancellationToken);
        return key;
    }

    public Task<Stream?> OpenReadAsync(string storageKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (!Guid.TryParseExact(Path.GetFileNameWithoutExtension(storageKey), "N", out _) ||
            Path.GetExtension(storageKey) != ".pdf" || Path.GetFileName(storageKey) != storageKey)
            return Task.FromResult<Stream?>(null);

        var path = Path.Combine(root, storageKey);
        return Task.FromResult<Stream?>(File.Exists(path) ? File.OpenRead(path) : null);
    }
}
