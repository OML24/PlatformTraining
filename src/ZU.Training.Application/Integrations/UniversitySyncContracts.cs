namespace ZU.Training.Application.Integrations;

public sealed record UniversitySyncResult(Guid RunId, bool Complete, int Added, int Updated,
    int Missing, string? Error);
public sealed record MissingEmployee(Guid Id, int EmployeeId, string NameAr,
    DateTimeOffset MissingSinceUtc, int? LastStatusCode);

public interface IUniversitySyncService
{
    Task<UniversitySyncResult> RefreshAsync(Guid actorId, string? ip, CancellationToken cancellationToken);
    Task<IReadOnlyList<UniversitySyncResult>> RecentRunsAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<MissingEmployee>> MissingEmployeesAsync(CancellationToken cancellationToken);
}
