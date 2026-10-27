using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ZU.Training.Infrastructure.Data;
using ZU.Training.Infrastructure.Integrations;
using ZU.Training.Infrastructure.Security;

namespace ZU.Training.Domain.Tests;

public sealed class UniversitySyncRunner
{
    [Fact]
    public async Task RunUniversitySyncLive()
    {
        var secretsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Microsoft", "UserSecrets", "57590e62-c854-4b39-b116-4d155a154e43", "secrets.json");

        if (!File.Exists(secretsPath)) return;

        using var doc = JsonDocument.Parse(File.ReadAllText(secretsPath));
        string? connStr = null;
        if (doc.RootElement.TryGetProperty("ConnectionStrings:TrainingDb", out var cProp))
            connStr = cProp.GetString();
        else if (doc.RootElement.TryGetProperty("ConnectionStrings", out var cObj) && cObj.TryGetProperty("TrainingDb", out var cNested))
            connStr = cNested.GetString();

        string? encKey = null;
        if (doc.RootElement.TryGetProperty("Security:EncryptionKey", out var eProp))
            encKey = eProp.GetString();
        else if (doc.RootElement.TryGetProperty("Security", out var sObj) && sObj.TryGetProperty("EncryptionKey", out var eNested))
            encKey = eNested.GetString();

        if (string.IsNullOrEmpty(connStr) || string.IsNullOrEmpty(encKey)) return;

        var options = new DbContextOptionsBuilder<TrainingDbContext>()
            .UseNpgsql(connStr)
            .Options;

        await using var db = new TrainingDbContext(options);
        var cipher = new AesCredentialCipher(encKey);
        using var client = new HttpClient { Timeout = TimeSpan.FromMinutes(10) };

        var endpoints = new UniversityDirectoryEndpoints(
            "https://eservices.zu.edu.jo/ords/fees/qa/employees",
            "https://eservices.zu.edu.jo/ords/fees/qa/departments",
            "https://eservices.zu.edu.jo/ords/fees/qa/faculties",
            "https://eservices.zu.edu.jo/ords/fees/qa/email"
        );

        var service = new UniversitySyncService(client, db, cipher, endpoints);

        var adminUser = await db.UserAccounts.FirstOrDefaultAsync(u => u.LoginName == "8303");
        var actorId = adminUser?.Id ?? Guid.NewGuid();

        var result = await service.RefreshAsync(actorId, "127.0.0.1", CancellationToken.None);

        Assert.True(result.Complete, "Sync must complete successfully. Error: " + result.Error);
        Assert.Null(result.Error);
        Assert.True(result.Added + result.Updated > 0, "Must add or update employees.");
    }
}
