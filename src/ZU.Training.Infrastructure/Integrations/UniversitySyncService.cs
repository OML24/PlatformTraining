using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Integrations;
using ZU.Training.Application.Security;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Settings;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Integrations;

public sealed record UniversityDirectoryEndpoints(string Employees, string Departments, string Faculties, string Centers, string Emails);

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
            var centerPage = await FetchAllAsync(endpoints.Centers, ct);
            var departmentPage = await FetchAllAsync(endpoints.Departments, ct);
            var emailPage = await FetchAllAsync(endpoints.Emails, ct);
            var employeePage = await FetchAllAsync(endpoints.Employees, ct);
            var complete = facultyPage.Complete && centerPage.Complete && departmentPage.Complete && emailPage.Complete && employeePage.Complete;
            var now = DateTimeOffset.UtcNow;
            db.Database.SetCommandTimeout(300);
            await using var transaction = await db.Database.BeginTransactionAsync(ct);

            var emails = new Dictionary<int, (string Email, string? Extension)>();
            foreach (var item in emailPage.Items)
            {
                var empId = Number(item, "employee_id");
                var email = Text(item, "email");
                var ext = Text(item, "extention");
                if (empId.HasValue && !string.IsNullOrWhiteSpace(email))
                    emails[empId.Value] = (email.Trim().ToLowerInvariant(), ext?.Trim());
            }

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

            var facultyByName = faculties.Values
                .GroupBy(f => f.NameAr.Replace("كلية", "").Trim())
                .ToDictionary(g => g.Key, g => g.First());

            // Process University Centers
            var centers = await db.UniversityCenters.ToDictionaryAsync(x => x.ExternalCode, ct);
            var facultyByCenterId = new Dictionary<string, Guid>();
            var deanEmployeeIds = new HashSet<int>();

            foreach (var item in centerPage.Items)
            {
                var code = Text(item, "center_id");
                if (string.IsNullOrWhiteSpace(code)) continue;
                var nameAr = Text(item, "center_dscp") ?? code;
                var headId = Number(item, "centr_head");
                var regId = Number(item, "fac_registrar");
                var unitId = Number(item, "unit_id");
                var sideCode = Number(item, "side");

                Guid? matchedFacultyId = null;
                var centerClean = nameAr.Replace("كلية", "").Trim();
                if (facultyByName.TryGetValue(centerClean, out var matchedFaculty))
                {
                    matchedFacultyId = matchedFaculty.Id;
                    facultyByCenterId[code] = matchedFaculty.Id;
                }
                else
                {
                    var partial = faculties.Values.FirstOrDefault(f =>
                        f.NameAr.Contains(centerClean) || centerClean.Contains(f.NameAr.Replace("كلية", "").Trim()));
                    if (partial != null)
                    {
                        matchedFacultyId = partial.Id;
                        facultyByCenterId[code] = partial.Id;
                    }
                }

                if (headId.HasValue && matchedFacultyId.HasValue)
                {
                    deanEmployeeIds.Add(headId.Value);
                }

                if (!centers.TryGetValue(code, out var center))
                {
                    center = new UniversityCenter
                    {
                        ExternalCode = code,
                        NameAr = nameAr,
                        HeadEmployeeId = headId,
                        RegistrarEmployeeId = regId,
                        UnitId = unitId,
                        SideCode = sideCode,
                        FacultyId = matchedFacultyId
                    };
                    db.UniversityCenters.Add(center);
                    centers[code] = center;
                }
                else
                {
                    center.NameAr = nameAr;
                    center.HeadEmployeeId = headId;
                    center.RegistrarEmployeeId = regId;
                    center.UnitId = unitId;
                    center.SideCode = sideCode;
                    center.FacultyId = matchedFacultyId;
                }
            }

            var departments = await db.Departments.ToDictionaryAsync(x => x.ExternalCode, ct);
            foreach (var item in departmentPage.Items)
            {
                var code = Text(item, "department_id") ?? Text(item, "departmentid");
                if (string.IsNullOrWhiteSpace(code)) continue;
                var centerCode = Text(item, "center_id");
                var regCode = Text(item, "reg_code");
                var facultyCode = Text(item, "facultyid") ?? (regCode is { Length: >= 2 } ? regCode[..2] : string.Empty);
                var headDepId = Number(item, "gate_dep_head");
                var deptNameAr = Text(item, "department_dscp") ?? Text(item, "departmentname") ?? code;
                var deptNameEn = Text(item, "departmentnamee");

                Guid? facultyId = null;
                if (!string.IsNullOrWhiteSpace(facultyCode) && faculties.TryGetValue(facultyCode, out var fac))
                    facultyId = fac.Id;
                else if (!string.IsNullOrWhiteSpace(centerCode) && facultyByCenterId.TryGetValue(centerCode, out var facId))
                    facultyId = facId;

                Guid? centerId = null;
                if (!string.IsNullOrWhiteSpace(centerCode) && centers.TryGetValue(centerCode, out var c))
                    centerId = c.Id;

                if (!departments.TryGetValue(code, out var department))
                {
                    department = new Department
                    {
                        ExternalCode = code,
                        FacultyExternalCode = facultyCode,
                        CenterExternalCode = centerCode,
                        CenterId = centerId,
                        FacultyId = facultyId,
                        NameAr = deptNameAr,
                        NameEn = deptNameEn,
                        HeadEmployeeId = headDepId,
                        RegCode = regCode
                    };
                    db.Departments.Add(department);
                    departments[code] = department;
                }
                else
                {
                    department.FacultyExternalCode = facultyCode;
                    department.CenterExternalCode = centerCode;
                    department.CenterId = centerId;
                    department.FacultyId = facultyId ?? department.FacultyId;
                    department.NameAr = deptNameAr;
                    if (!string.IsNullOrWhiteSpace(deptNameEn)) department.NameEn = deptNameEn;
                    department.HeadEmployeeId = headDepId ?? department.HeadEmployeeId;
                    if (!string.IsNullOrWhiteSpace(regCode)) department.RegCode = regCode;
                }
            }

            // Persist faculties, centers and departments so their generated/existing IDs are committed for foreign keys
            await db.SaveChangesAsync(ct);

            var deptByName = departments.Values
                .GroupBy(d => d.NameAr.Trim())
                .ToDictionary(g => g.Key, g => g.First());

            var rolesByKey = await db.Roles.ToDictionaryAsync(x => x.Key, ct);
            var memberRole = rolesByKey["MEMBER"];
            var deanRole = rolesByKey.GetValueOrDefault("DEAN");
            var qaRole = rolesByKey.GetValueOrDefault("QA");

            var existingRolesByUser = await (from ur in db.UserRoles
                                             where ur.IsActive
                                             select new { ur.UserId, ur.RoleId })
                                             .GroupBy(x => x.UserId)
                                             .ToDictionaryAsync(g => g.Key, g => g.Select(x => x.RoleId).ToHashSet(), ct);

            var users = await db.UserAccounts.Where(x => x.EmployeeId.HasValue)
                .ToDictionaryAsync(x => x.EmployeeId!.Value, ct);
            var existingLoginNames = await db.UserAccounts.Select(x => x.LoginName).ToHashSetAsync(ct);
            var policy = await db.PasswordPolicies.AsNoTracking().Where(x => x.IsActive)
                .OrderByDescending(x => x.EffectiveAtUtc).FirstAsync(ct);
            var seen = new HashSet<int>();

            db.ChangeTracker.AutoDetectChangesEnabled = false;
            foreach (var item in employeePage.Items)
            {
                var employeeId = Number(item, "employee_id");
                if (!employeeId.HasValue || employeeId <= 0 || !seen.Add(employeeId.Value)) continue;

                var sourceDeptId = Text(item, "department_id");
                var sourceCenterId = Text(item, "center_id");
                var isTeaching = Number(item, "main_job_code") == 1;
                var subJob = Text(item, "sub_job_dscp") ?? "";
                var centerDesc = Text(item, "center_dscp") ?? "";
                var deptDesc = Text(item, "department_dscp") ?? "";

                Guid? matchedDeptId = null;
                Guid? matchedFacultyId = null;

                // 1. Resolve department by code first
                if (!string.IsNullOrWhiteSpace(sourceDeptId) && departments.TryGetValue(sourceDeptId, out var deptByCode))
                {
                    matchedDeptId = deptByCode.Id;
                    matchedFacultyId = deptByCode.FacultyId;
                }
                // 2. Or by name
                else if (!string.IsNullOrWhiteSpace(deptDesc) && deptByName.TryGetValue(deptDesc.Trim(), out var matchedDept))
                {
                    matchedDeptId = matchedDept.Id;
                    matchedFacultyId = matchedDept.FacultyId;
                }

                // 3. If faculty is still null, resolve from sourceCenterId via centers map
                if (matchedFacultyId == null && !string.IsNullOrWhiteSpace(sourceCenterId))
                {
                    if (facultyByCenterId.TryGetValue(sourceCenterId, out var facId))
                    {
                        matchedFacultyId = facId;
                    }
                    else if (faculties.TryGetValue(sourceCenterId, out var facByCenter))
                    {
                        matchedFacultyId = facByCenter.Id;
                    }
                }

                // 4. Or from center description
                if (matchedFacultyId == null)
                {
                    var centerKey = centerDesc.Replace("كلية", "").Trim();
                    if (!string.IsNullOrWhiteSpace(centerKey) && facultyByName.TryGetValue(centerKey, out var matchedFaculty))
                    {
                        matchedFacultyId = matchedFaculty.Id;
                    }
                }

                var isDean = (isTeaching && subJob.Contains("عميد") && !subJob.Contains("مساعد") && !subJob.Contains("نائب"))
                             || deanEmployeeIds.Contains(employeeId.Value);
                var isQa = subJob.Contains("جودة") || centerDesc.Contains("جودة") || deptDesc.Contains("جودة");
                var emailInfo = emails.GetValueOrDefault(employeeId.Value);
                var email = emailInfo.Email;
                var extension = emailInfo.Extension;
                var mobile = Text(item, "mobile")?.Trim();
                var jobTitle = (Text(item, "sub_job_dscp") ?? Text(item, "main_job_dscp"))?.Trim();

                if (!users.TryGetValue(employeeId.Value, out var user))
                {
                    var login = employeeId.Value.ToString();
                    if (existingLoginNames.Contains(login))
                        continue;
                    existingLoginNames.Add(login);
                    var initial = GeneratePassword(login, policy);
                    user = new UserAccount
                    {
                        EmployeeId = employeeId.Value,
                        LoginName = login,
                        NameAr = ArabicName(item) ?? login,
                        NameEn = Text(item, "ename"),
                        Email = email,
                        Mobile = mobile,
                        PhoneExtension = extension,
                        JobTitle = jobTitle,
                        EncryptedInitialPassword = cipher.Encrypt(initial),
                        EncryptedCurrentPassword = cipher.Encrypt(initial),
                        FacultyId = matchedFacultyId,
                        DepartmentId = matchedDeptId,
                        SourceDepartmentId = sourceDeptId,
                        SourceCenterId = sourceCenterId,
                        SourceEmploymentStatusCode = Number(item, "emp_status"),
                        SourceEmploymentCheck = Number(item, "chk_emp_stat"),
                        SourceMainJobCode = Number(item, "main_job_code"),
                        SourceSideCode = Number(item, "side"),
                        SourceLastSeenAtUtc = now,
                        AccessOverrideAllow = (Number(item, "emp_status") == 13)
                    };
                    db.UserAccounts.Add(user);
                    users[employeeId.Value] = user;
                    run.AddedCount++;

                    if (isTeaching)
                        db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = memberRole.Id });
                    if (isDean && deanRole is not null)
                        db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = deanRole.Id });
                    if (isQa && qaRole is not null)
                        db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = qaRole.Id });
                }
                else
                {
                    run.UpdatedCount++;
                    if (!string.IsNullOrWhiteSpace(email)) user.Email = email;
                    if (!string.IsNullOrWhiteSpace(mobile)) user.Mobile = mobile;
                    if (!string.IsNullOrWhiteSpace(extension)) user.PhoneExtension = extension;
                    if (!string.IsNullOrWhiteSpace(jobTitle)) user.JobTitle = jobTitle;
                    if (matchedFacultyId.HasValue) user.FacultyId = matchedFacultyId;
                    if (matchedDeptId.HasValue) user.DepartmentId = matchedDeptId;
                    if (!existingRolesByUser.TryGetValue(user.Id, out var existingRoleIds) || existingRoleIds.Count == 0)
                    {
                        if (isTeaching)
                            db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = memberRole.Id });
                        if (isDean && deanRole is not null)
                            db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = deanRole.Id });
                        if (isQa && qaRole is not null)
                            db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = qaRole.Id });
                    }
                    else if (isDean && deanRole is not null && !existingRoleIds.Contains(deanRole.Id))
                    {
                        db.UserRoles.Add(new UserRole { UserId = user.Id, RoleId = deanRole.Id });
                    }
                }
                user.NameAr = ArabicName(item) ?? user.NameAr;
                user.NameEn = Text(item, "ename") ?? user.NameEn;
                user.SourceEmploymentStatusCode = Number(item, "emp_status");
                user.SourceEmploymentCheck = Number(item, "chk_emp_stat");
                user.SourceMainJobCode = Number(item, "main_job_code");
                user.SourceSideCode = Number(item, "side");
                user.SourceDepartmentId = sourceDeptId;
                user.SourceCenterId = sourceCenterId;
                if (matchedFacultyId.HasValue) user.FacultyId = matchedFacultyId;
                if (matchedDeptId.HasValue) user.DepartmentId = matchedDeptId;
                if (!string.IsNullOrWhiteSpace(email)) user.Email = email;
                if (!string.IsNullOrWhiteSpace(mobile)) user.Mobile = mobile;
                if (!string.IsNullOrWhiteSpace(extension)) user.PhoneExtension = extension;
                if (!string.IsNullOrWhiteSpace(jobTitle)) user.JobTitle = jobTitle;
                user.SourceLastSeenAtUtc = now;
                user.SourceMissingSinceUtc = null;
            }
            db.ChangeTracker.AutoDetectChangesEnabled = true;

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
            byte[] bytes = [];
            for (var attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    using var response = await client.GetAsync(next, ct);
                    response.EnsureSuccessStatusCode();
                    bytes = await response.Content.ReadAsByteArrayAsync(ct);
                    break;
                }
                catch when (attempt < 3)
                {
                    await Task.Delay(1500 * attempt, ct);
                }
            }
            using var document = JsonDocument.Parse(bytes);
            var root = document.RootElement;
            if (!root.TryGetProperty("items", out var sourceItems) || sourceItems.ValueKind != JsonValueKind.Array)
                throw new InvalidDataException("University API response has no items array.");
            var current = sourceItems.EnumerateArray().Select(x => x.Clone()).ToArray();
            items.AddRange(current);
            if (!root.TryGetProperty("hasMore", out var more) || more.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
                return new Page(items, true);
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
