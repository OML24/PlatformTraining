namespace ZU.Training.Application.Training;

public interface ICertificateStorage
{
    Task<string> SavePdfAsync(byte[] bytes, CancellationToken cancellationToken);
    Task<Stream?> OpenReadAsync(string storageKey, CancellationToken cancellationToken);
}
