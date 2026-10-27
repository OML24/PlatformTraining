using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Integrations;
using ZU.Training.Application.Security;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Settings;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Integrations;

public sealed record UniversityDirectoryEndpoints(string Employees, string Departments, string Faculties);

public sealed class UniversitySyncService(HttpClient client, TrainingDbContext db,
    ICredentialCipher cipher, UniversityDirectoryEndpoints endpoints) : IUniversitySyncService
{
    private sealed record Page(IReadOnlyList<JsonElement> Items, bool Complete);

    public async Task<UniversitySyncResult> RefreshAsync(Guid actorId, string? ip, CancellationToken ct)
    {
        var run = new IntegrationSyncRun { SourceKey = "university-directory" };
        db.IntegrationSyncRuns.Add(run);
        await db.SaveChangesAsync(ct);
        try
        {
            var facultyPage = await FetchAllAsync(endpoints.Faculties, ct);
            var departmentPage = await FetchAllAsync(endpoints.Departments, ct);
            var employeePage = await FetchAllAsync(endpoints.Employees, ct);
            var complete = facultyPage.Complete && departmentPage.Complete && employeePage.Complete;
            var now = DateTimeOffset.UtcNow;
            await using var transaction = await db.Database.BeginTransactionAsync(ct);

            var faculties = await db.Faculties.ToDictionaryAsync(x => x.ExternalCode, ct);
            foreach (var item in facultyPage.Items)
            {
                var code = Text(item, "code"); if (string.IsNullOrWhiteSpace(code)) continue;
                if (!faculties.TryGetValue(code, out var faculty))
                {
                    faculty = new Faculty { ExternalCode = code, NameAr = Text(item, "faculty") ?? code };
                    db.Faculties.Add(faculty); faculties[code] = faculty;
                }
                faculty.NameAr = Text(item, "faculty") ?? faculty.NameAr;
                faculty.NameEn = Text(item, "facultynamee");
            }

            var departments = await db.Departments.ToDictionaryAsync(x => x.ExternalCode, ct);
            foreach (var item in departmentPage.Items)
            {
                var code = Text(item, "departmentid"); if (string.IsNullOrWhiteSpace(code)) continue;
                var facultyCode = Text(item, "facultyid") ?? string.Empty;
                if (!departments.TryGetValue(code, out var department))
                {
                    department = new Department { ExternalCode = code, FacultyExternalCode = facultyCode,
                        NameAr = Text(item, "departmentname") ?? code };
                    db.Departments.Add(department); departments[code] = department;
                }
                department.FacultyExternalCode = facultyCode;
                department.FacultyId = faculties.GetValueOrDefault(facultyCode)?.Id;
                department.NameAr = Text(item, "departmentname") ?? department.NameAr;
                department.NameEn = Text(item, "departmentnamee");
            }

            var users = await db.UserAccounts.Where(x => x.EmployeeId.HasValue)
                .ToDictionaryAsync(x => x.EmployeeId!.Value, ct);
            var memberRole = await db.Roles.SingleAsync(x => x.Key == "MEMBER", ct);
            var policy = await db.PasswordPolicies.AsNoTracking().Where(x => x.IsActive)
                .OrderByDescending(x => x.EffectiveAtUtc).FirstAsync(ct);
            var seen = new HashSet<int>();
            foreach (var item in employeePage.Items)
            {
                var employeeId = Number(item, "employee_id");
                if (!employeeId.HasValue || employeeId <= 0 || !seen.Add(employeeId.Value)) continue;
                if (!users.TryGetValue(employeeId.Value, out var user))
                {
                    var login = employeeId.Value.ToString();
                    if (await db.UserAccounts.AnyAsync(x => x.LoginName == login, ct))
                        continue;
                    var initial = GeneratePassword(login, policy);
                    user = new UserAccount
                    {
                        EmployeeId = employeeId.Value, LoginName = login,
                        NameAr = ArabicName(item) ?? login,
                        EncryptedInitialPassword = cipher.Encrypt(initial),
                        EncryptedCurrentPassword = cipher.Encrypt(initial)
                    };
                    db.UserAccounts.Add(user); users[employeeId.Value] = user;
                    run.AddedCount++;
                    if (Number(item, "main_job_code") == 1)
                        db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = memberRole.Id });
                }
                else run.UpdatedCount++;
                user.NameAr = ArabicName(item) ?? user.NameAr;
                user.NameEn = Text(item, "ename");
                user.SourceEmploymentStatusCode = Number(item, "emp_status");
                user.SourceEmploymentCheck = Number(item, "chk_emp_stat");
                user.SourceMainJobCode = Number(item, "main_job_code");
                user.SourceSideCode = Number(item, "side");
                user.SourceDepartmentId = Text(item, "department_id");
                user.SourceCenterId = Text(item, "center_id");
                user.SourceLastSeenAtUtc = now;
                user.SourceMissingSinceUtc = null;
            }
            if (complete)
            {
                foreach (var user in users.Values.Where(x => x.SourceLastSeenAtUtc.HasValue &&
                             !seen.Contains(x.EmployeeId!.Value)))
                {
                    user.SourceMissingSinceUtc ??= now;
                    run.MissingCount++;
                }
            }
            run.WasComplete = complete;
            run.FinishedAtUtc = now;
            db.AuditEvents.Add(new AuditEvent { ActorUserId = actorId, ActionKey = "university.sync",
                EntityType = "IntegrationSyncRun", EntityId = run.Id, IpAddress = ip,
                Reason = complete ? "complete" : "incomplete-source-pagination" });
            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            return new UniversitySyncResult(run.Id, complete, run.AddedCount, run.UpdatedCount,
                run.MissingCount, complete ? null : "Source pagination did not confirm a complete result.");
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            db.ChangeTracker.Clear();
            var failedRun = await db.IntegrationSyncRuns.SingleAsync(x => x.Id == run.Id, ct);
            failedRun.FinishedAtUtc = DateTimeOffset.UtcNow;
            failedRun.ErrorSummary = ex.GetType().Name + ": " + ex.Message[..Math.Min(ex.Message.Length, 500)];
            failedRun.WasComplete = false;
            await db.SaveChangesAsync(ct);
            return new UniversitySyncResult(run.Id, false, 0, 0, 0, failedRun.ErrorSummary);
        }
    }

    public async Task<IReadOnlyList<UniversitySyncResult>> RecentRunsAsync(CancellationToken ct)
        => await db.IntegrationSyncRuns.AsNoTracking().OrderByDescending(x => x.StartedAtUtc).Take(20)
            .Select(x => new UniversitySyncResult(x.Id, x.WasComplete, x.AddedCount,
                x.UpdatedCount, x.MissingCount, x.ErrorSummary)).ToListAsync(ct);

    public async Task<IReadOnlyList<MissingEmployee>> MissingEmployeesAsync(CancellationToken ct)
        => await db.UserAccounts.AsNoTracking().Where(x => x.EmployeeId.HasValue &&
                x.SourceMissingSinceUtc.HasValue)
            .OrderBy(x => x.NameAr).Select(x => new MissingEmployee(x.Id, x.EmployeeId!.Value,
                x.NameAr, x.SourceMissingSinceUtc!.Value, x.SourceEmploymentStatusCode)).ToListAsync(ct);

    private async Task<Page> FetchAllAsync(string url, CancellationToken ct)
    {
        var baseUri = new Uri(url);
        var next = baseUri;
        var items = new List<JsonElement>();
        for (var page = 0; page < 1000; page++)
        {
            using var response = await client.GetAsync(next, ct);
            response.EnsureSuccessStatusCode();
            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
            var root = document.RootElement;
            if (!root.TryGetProperty("items", out var sourceItems) || sourceItems.ValueKind != JsonValueKind.Array)
                throw new InvalidDataException("University API response has no items array.");
            var current = sourceItems.EnumerateArray().Select(x => x.Clone()).ToArray();
            items.AddRange(current);
            if (!root.TryGetProperty("hasMore", out var more) || more.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
                return new Page(items, false);
            if (more.ValueKind == JsonValueKind.False) return new Page(items, true);
            if (current.Length == 0) throw new InvalidDataException("University API pagination did not advance.");
            var nextHref = root.TryGetProperty("links", out var links) && links.ValueKind == JsonValueKind.Array
                ? links.EnumerateArray().FirstOrDefault(x => Text(x, "rel") == "next")
                : default;
            var href = nextHref.ValueKind == JsonValueKind.Object ? Text(nextHref, "href") : null;
            if (href is not null)
            {
                next = new Uri(next, href);
                if (!string.Equals(next.Host, baseUri.Host, StringComparison.OrdinalIgnoreCase))
                    throw new InvalidDataException("University API pagination changed host.");
            }
            else
            {
                var offset = Number(root, "offset") ?? (page == 0 ? 0 : items.Count - current.Length);
                var builder = new UriBuilder(baseUri);
                var query = builder.Query.TrimStart('?');
                query = string.Join('&', query.Split('&', StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !x.StartsWith("offset=", StringComparison.OrdinalIgnoreCase)));
                builder.Query = (query.Length == 0 ? "" : query + "&") + "offset=" + (offset + current.Length);
                next = builder.Uri;
            }
        }
        throw new InvalidDataException("University API pagination exceeded the page limit.");
    }

    private static string? Text(JsonElement item, string property)
    {
        if (item.ValueKind != JsonValueKind.Object || !item.TryGetProperty(property, out var value) ||
            value.ValueKind == JsonValueKind.Null) return null;
        return (value.ValueKind == JsonValueKind.String ? value.GetString() : value.ToString())?.Trim();
    }
    private static int? Number(JsonElement item, string property)
        => int.TryParse(Text(item, property), out var result) ? result : null;

    private static string? ArabicName(JsonElement item)
    {
        var parts = new[] { "fname", "sname", "tname", "lname" }
            .Select(key => Text(item, key)).Where(x => !string.IsNullOrWhiteSpace(x));
        var name = string.Join(' ', parts);
        return name.Length == 0 ? null : name;
    }

    private static string GeneratePassword(string login, PasswordPolicy policy)
    {
        const string alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
        var length = Math.Max(policy.MinimumLength, 20);
        if (length > policy.MaximumLength) throw new InvalidOperationException("Password policy has no valid length.");
        for (var attempt = 0; attempt < 100; attempt++)
        {
            var chars = new char[length];
            for (var i = 0; i < chars.Length; i++) chars[i] = alphabet[RandomNumberGenerator.GetInt32(alphabet.Length)];
            var password = new string(chars);
            if (AuthService.IsAllowedByPolicy(password, login, policy)) return password;
        }
        throw new InvalidOperationException("Could not generate an initial password.");
    }
}
