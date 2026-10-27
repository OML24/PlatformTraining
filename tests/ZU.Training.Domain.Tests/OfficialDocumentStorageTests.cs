using System.IO.Compression;
using System.Text;
using ZU.Training.Infrastructure.Storage;

namespace ZU.Training.Domain.Tests;

public sealed class OfficialDocumentStorageTests
{
    [Fact]
    public async Task Stores_pdf_and_sanitizes_windows_path_from_client()
    {
        var root = Path.Combine(Path.GetTempPath(), "zu-training-test-" + Guid.NewGuid().ToString("N"));
        try
        {
            var storage = new OfficialDocumentStorage(root);
            using var source = new MemoryStream(Encoding.ASCII.GetBytes("%PDF-1.7\nTest"));
            var saved = await storage.SaveAsync(source, "C:\\uploads\\impact.pdf", CancellationToken.None);
            Assert.Equal("impact.pdf", saved.OriginalName);
            await using (var result = await storage.OpenReadAsync(saved.StorageKey, CancellationToken.None))
            {
                Assert.NotNull(result);
                Assert.Equal(source.Length, result.Length);
            }
            await storage.DeleteAsync(saved.StorageKey, CancellationToken.None);
            Assert.Null(await storage.OpenReadAsync(saved.StorageKey, CancellationToken.None));
        }
        finally { if (Directory.Exists(root)) Directory.Delete(root, true); }
    }

    [Fact]
    public async Task Rejects_docx_without_document_part_and_removes_partial_file()
    {
        var root = Path.Combine(Path.GetTempPath(), "zu-training-test-" + Guid.NewGuid().ToString("N"));
        try
        {
            using var source = new MemoryStream();
            using (var archive = new ZipArchive(source, ZipArchiveMode.Create, leaveOpen: true))
            {
                archive.CreateEntry("[Content_Types].xml");
                archive.CreateEntry("other.xml");
            }
            source.Position = 0;
            var storage = new OfficialDocumentStorage(root);
            await Assert.ThrowsAsync<InvalidDataException>(() =>
                storage.SaveAsync(source, "impact.docx", CancellationToken.None));
            Assert.Empty(Directory.GetFiles(root));
        }
        finally { if (Directory.Exists(root)) Directory.Delete(root, true); }
    }
}
