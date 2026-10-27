using System.Security.Cryptography;
using ZU.Training.Application.Security;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Settings;

namespace ZU.Training.Application.Administration;

public sealed class UserAdminService(IUserAdminStore store, ICredentialCipher cipher)
{
    public async Task<IReadOnlyList<UserAdminSummary>> SearchAsync(string query, int page, CancellationToken ct)
    {
        if (page < 1 || page > 100000) throw new ArgumentException("Invalid page.");
        var users = await store.SearchUsersAsync(query.Trim(), (page - 1) * 50, 50, ct);
        var roles = await store.RolesAsync(ct);
        var summaries = new List<UserAdminSummary>();
        foreach (var user in users)
        {
            var keys = (await store.UserRolesAsync(user.Id, ct)).Where(x => x.IsActive)
                .Join(roles, x => x.RoleId, x => x.Id, (_, role) => role.Key).ToArray();
            summaries.Add(new UserAdminSummary(user.Id, user.LoginName, user.NameAr, user.NameEn,
                user.Email, user.IsActive, user.IsExternalTrainer, user.FirstLoginCompleted,
                user.SourceEmploymentStatusCode, user.AccessOverrideAllow, user.FacultyId, keys));
        }
        return summaries;
    }

    public async Task<UserAdminSummary> CreateAsync(Guid actorId, CreateUserInput input, string? ip, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(input.NameAr) || input.NameAr.Length > 300 ||
            input.NameEn?.Length > 300 || input.Email?.Length > 320)
            throw new ArgumentException("Invalid account details.");
        var roles = await ResolveRolesAsync(input.RoleKeys, ct);
        var login = input.IsExternalTrainer ? input.ExternalLoginName?.Trim() : input.EmployeeId?.ToString();
        if (input.IsExternalTrainer)
        {
            if (login is not null && (login.Length != 4 || !login.All(char.IsAsciiDigit)))
                throw new ArgumentException("External trainer login must contain four digits.");
            if (login is null)
            {
                for (var attempt = 0; attempt < 100; attempt++)
                {
                    var candidate = RandomNumberGenerator.GetInt32(1000, 10000).ToString();
                    if (!await store.LoginExistsAsync(candidate, ct)) { login = candidate; break; }
                }
                if (login is null) throw new InvalidOperationException("No four digit login is available.");
            }
            if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.ExternalPassword))
                throw new ArgumentException("External trainer email and password are required.");
            var policy = await store.CurrentPolicyAsync(ct) ?? throw new InvalidOperationException("Password policy missing.");
            if (!AuthService.IsAllowedByPolicy(input.ExternalPassword, login, policy))
                throw new ArgumentException("Password does not meet the current policy.");
            var externalRole = (await store.RolesAsync(ct)).Single(x => x.Key == "EXTERNAL_INSTRUCTOR");
            if (roles.All(x => x.Id != externalRole.Id)) roles.Add(externalRole);
        }
        else if (input.EmployeeId is null or <= 0 || login is null)
            throw new ArgumentException("Employee number is required.");
        if (await store.LoginExistsAsync(login, ct))
            throw new InvalidOperationException("Login name already exists.");

        var initial = input.IsExternalTrainer ? null : await GenerateInitialPasswordAsync(login, ct);
        var user = new UserAccount
        {
            LoginName = login, EmployeeId = input.IsExternalTrainer ? null : input.EmployeeId,
            NameAr = input.NameAr.Trim(), NameEn = Clean(input.NameEn), Email = Clean(input.Email),
            IsExternalTrainer = input.IsExternalTrainer, FirstLoginCompleted = input.IsExternalTrainer,
            EncryptedInitialPassword = initial is null ? null : cipher.Encrypt(initial),
            EncryptedCurrentPassword = cipher.Encrypt(input.ExternalPassword ?? initial!),
            FacultyId = input.FacultyId, DepartmentId = input.DepartmentId,
            AccessOverrideAllow = !input.IsExternalTrainer
        };
        store.AddUser(user);
        foreach (var role in roles) store.AddUserRole(new UserRole { UserId = user.Id, RoleId = role.Id });
        Audit(actorId, "user.created", "UserAccount", user.Id, ip);
        await store.SaveAsync(ct);
        return new UserAdminSummary(user.Id, user.LoginName, user.NameAr, user.NameEn, user.Email,
            user.IsActive, user.IsExternalTrainer, user.FirstLoginCompleted, user.SourceEmploymentStatusCode,
            user.AccessOverrideAllow, user.FacultyId, roles.Select(x => x.Key).ToArray());
    }

    public async Task<UserCredentialView> CredentialsAsync(Guid actorId, Guid targetId, string? ip, CancellationToken ct)
    {
        var user = await RequiredUserAsync(targetId, ct);
        Audit(actorId, "user.credentials.viewed", "UserAccount", targetId, ip);
        await store.SaveAsync(ct);
        return new UserCredentialView(user.EncryptedInitialPassword is null ? null : cipher.Decrypt(user.EncryptedInitialPassword),
            cipher.Decrypt(user.EncryptedCurrentPassword));
    }

    public async Task ChangePasswordAsync(Guid actorId, Guid targetId, string newPassword, string? ip, CancellationToken ct)
    {
        var user = await RequiredUserAsync(targetId, ct);
        var policy = await store.CurrentPolicyAsync(ct) ?? throw new InvalidOperationException("Password policy missing.");
        if (!AuthService.IsAllowedByPolicy(newPassword, user.LoginName, policy))
            throw new ArgumentException("Password does not meet the current policy.");
        user.EncryptedCurrentPassword = cipher.Encrypt(newPassword);
        user.AuthVersion++;
        Audit(actorId, "user.password.changed", "UserAccount", targetId, ip);
        await store.SaveAsync(ct);
    }

    public async Task SetActiveAsync(Guid actorId, Guid targetId, bool active, string? ip, CancellationToken ct)
    {
        var user = await RequiredUserAsync(targetId, ct);
        user.IsActive = active; user.AuthVersion++;
        Audit(actorId, active ? "user.restored" : "user.disabled", "UserAccount", targetId, ip);
        await store.SaveAsync(ct);
    }

    public async Task SetAccessOverrideAsync(Guid actorId, Guid targetId, bool allowed, string? ip, CancellationToken ct)
    {
        var user = await RequiredUserAsync(targetId, ct);
        user.AccessOverrideAllow = allowed; user.AuthVersion++;
        Audit(actorId, "user.access_override.changed", "UserAccount", targetId, ip);
        await store.SaveAsync(ct);
    }

    public async Task SetRolesAsync(Guid actorId, Guid targetId, IReadOnlyList<string> keys, string? ip, CancellationToken ct)
    {
        await RequiredUserAsync(targetId, ct);
        var roles = await ResolveRolesAsync(keys, ct);
        var existing = await store.UserRolesAsync(targetId, ct);
        foreach (var row in existing) row.IsActive = roles.Any(x => x.Id == row.RoleId);
        foreach (var role in roles.Where(role => existing.All(x => x.RoleId != role.Id)))
            store.AddUserRole(new UserRole { UserId = targetId, RoleId = role.Id });
        var user = await RequiredUserAsync(targetId, ct);
        user.AuthVersion++;
        Audit(actorId, "user.roles.changed", "UserAccount", targetId, ip);
        await store.SaveAsync(ct);
    }

    public async Task<IReadOnlyList<PermissionState>> PermissionMatrixAsync(Guid targetId, CancellationToken ct)
    {
        await RequiredUserAsync(targetId, ct);
        var roles = await store.RolesAsync(ct);
        var userRoles = (await store.UserRolesAsync(targetId, ct)).Where(x => x.IsActive).ToList();
        var roleIds = userRoles.Select(x => x.RoleId).ToHashSet();
        var hasAdmin = roles.Any(x => x.Key == "ADMIN" && roleIds.Contains(x.Id));
        var grants = (await store.RolePermissionsAsync(ct)).Where(x => x.IsActive && roleIds.Contains(x.RoleId))
            .Select(x => x.PermissionId).ToHashSet();
        var overrides = (await store.OverridesAsync(targetId, ct)).Where(x => x.IsActive)
            .ToDictionary(x => x.PermissionId, x => x.Mode);
        var permissions = await store.PermissionsAsync(ct);
        return permissions.Where(x => x.IsActive).Select(p =>
        {
            var inherited = hasAdmin || grants.Contains(p.Id);
            var mode = overrides.GetValueOrDefault(p.Id, PermissionOverrideMode.Inherit);
            return new PermissionState(p.Key, p.NameAr, p.NameEn, inherited, mode,
                PermissionDecision.IsAllowed(inherited, mode));
        }).ToList();
    }

    public async Task SetPermissionOverrideAsync(Guid actorId, Guid targetId, string key,
        PermissionOverrideMode mode, string? ip, CancellationToken ct)
    {
        await RequiredUserAsync(targetId, ct);
        if (!Enum.IsDefined(mode)) throw new ArgumentException("Invalid override mode.");
        var permission = await store.PermissionAsync(key, ct) ?? throw new KeyNotFoundException("Permission not found.");
        var existing = (await store.OverridesAsync(targetId, ct)).FirstOrDefault(x => x.PermissionId == permission.Id);
        if (existing is null) store.AddOverride(new UserPermissionOverride
        {
            UserId = targetId, PermissionId = permission.Id, Mode = mode
        });
        else { existing.Mode = mode; existing.IsActive = true; }
        Audit(actorId, "user.permission_override.changed", "UserAccount", targetId, ip, key + ":" + mode);
        await store.SaveAsync(ct);
    }

    public async Task<IReadOnlyList<Role>> RolesAsync(CancellationToken ct) => await store.RolesAsync(ct);

    public async Task<IReadOnlyList<RolePermissionState>> RoleMatrixAsync(string roleKey, CancellationToken ct)
    {
        var role = (await store.RolesAsync(ct)).FirstOrDefault(x => x.Key == roleKey)
            ?? throw new KeyNotFoundException("Role not found.");
        var granted = (await store.RolePermissionsAsync(ct)).Where(x => x.RoleId == role.Id)
            .Select(x => x.PermissionId).ToHashSet();
        var permissions = await store.PermissionsAsync(ct);
        return permissions.Select(x => new RolePermissionState(x.Key, x.NameAr, x.NameEn,
            role.Key == "ADMIN" || granted.Contains(x.Id))).ToList();
    }

    public async Task SetRolePermissionAsync(Guid actorId, string roleKey, string permissionKey,
        bool granted, string? ip, CancellationToken ct)
    {
        var role = (await store.RolesAsync(ct)).FirstOrDefault(x => x.Key == roleKey)
            ?? throw new KeyNotFoundException("Role not found.");
        if (role.Key == "ADMIN") throw new InvalidOperationException("Administrator inherits all permissions.");
        var permission = await store.PermissionAsync(permissionKey, ct)
            ?? throw new KeyNotFoundException("Permission not found.");
        var existing = await store.RolePermissionAsync(role.Id, permission.Id, ct);
        if (existing is null && granted)
            store.AddRolePermission(new RolePermission { RoleId = role.Id, PermissionId = permission.Id });
        else if (existing is not null) existing.IsActive = granted;
        Audit(actorId, "role.permission.changed", "Role", role.Id, ip,
            permissionKey + ":" + (granted ? "allow" : "deny"));
        await store.SaveAsync(ct);
    }

    public async Task<string?> SharedInitialPasswordAsync(Guid actorId, string? ip, CancellationToken ct)
    {
        var shared = await store.SharedPasswordAsync(ct);
        Audit(actorId, "shared_initial_password.viewed", "SharedInitialPassword", shared?.Id, ip);
        await store.SaveAsync(ct);
        return shared is null ? null : cipher.Decrypt(shared.EncryptedPassword);
    }

    public async Task SetSharedInitialPasswordAsync(Guid actorId, string password, string? ip, CancellationToken ct)
    {
        var policy = await store.CurrentPolicyAsync(ct) ?? throw new InvalidOperationException("Password policy missing.");
        if (!AuthService.IsAllowedByPolicy(password, string.Empty, policy))
            throw new ArgumentException("Password does not meet the current policy.");
        var shared = await store.SharedPasswordAsync(ct);
        if (shared is null)
        {
            shared = new SharedInitialPassword { EncryptedPassword = cipher.Encrypt(password) };
            store.AddSharedPassword(shared);
        }
        else shared.EncryptedPassword = cipher.Encrypt(password);
        Audit(actorId, "shared_initial_password.changed", "SharedInitialPassword", shared.Id, ip);
        await store.SaveAsync(ct);
    }

    private async Task<UserAccount> RequiredUserAsync(Guid id, CancellationToken ct)
        => await store.UserAsync(id, ct) ?? throw new KeyNotFoundException("User not found.");

    private async Task<List<Role>> ResolveRolesAsync(IReadOnlyList<string> keys, CancellationToken ct)
    {
        var all = await store.RolesAsync(ct);
        var selected = all.Where(x => keys.Contains(x.Key, StringComparer.Ordinal) && x.IsActive).ToList();
        if (selected.Count != keys.Distinct(StringComparer.Ordinal).Count())
            throw new ArgumentException("One or more roles are invalid.");
        return selected;
    }

    private async Task<string> GenerateInitialPasswordAsync(string login, CancellationToken ct)
    {
        var policy = await store.CurrentPolicyAsync(ct) ?? throw new InvalidOperationException("Password policy missing.");
        const string alphabet = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
        var length = Math.Max(policy.MinimumLength, 20);
        if (length > policy.MaximumLength) throw new InvalidOperationException("Password policy has no valid length.");
        for (var attempt = 0; attempt < 100; attempt++)
        {
            var chars = new char[length];
            for (var i = 0; i < chars.Length; i++) chars[i] = alphabet[RandomNumberGenerator.GetInt32(alphabet.Length)];
            var candidate = new string(chars);
            if (AuthService.IsAllowedByPolicy(candidate, login, policy)) return candidate;
        }
        throw new InvalidOperationException("Could not generate a password that meets the policy.");
    }

    private void Audit(Guid actorId, string action, string entityType, Guid? entityId, string? ip, string? reason = null)
        => store.AddAudit(new AuditEvent { ActorUserId = actorId, ActionKey = action, EntityType = entityType,
            EntityId = entityId, IpAddress = ip, Reason = reason });

    private static string? Clean(string? value) => string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
