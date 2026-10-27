using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Security;
using ZU.Training.Domain.Identity;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Security;

public sealed class EfPermissionAccess(TrainingDbContext db) : IPermissionAccess
{
    public async Task<bool> IsAllowedAsync(Guid userId, string permissionKey, CancellationToken cancellationToken)
    {
        var permission = await db.Permissions
            .Where(x => x.Key == permissionKey && x.IsActive)
            .Select(x => new { x.Id })
            .SingleOrDefaultAsync(cancellationToken);
        if (permission is null)
            return false;

        var mode = await db.UserPermissionOverrides
            .Where(x => x.UserId == userId && x.PermissionId == permission.Id && x.IsActive)
            .Select(x => (PermissionOverrideMode?)x.Mode)
            .SingleOrDefaultAsync(cancellationToken) ?? PermissionOverrideMode.Inherit;

        var inherited = await (from userRole in db.UserRoles
                               join role in db.Roles on userRole.RoleId equals role.Id
                               where userRole.UserId == userId && userRole.IsActive && role.IsActive
                               select role.Key).AnyAsync(key => key == "ADMIN", cancellationToken);
        if (!inherited)
            inherited = await (from userRole in db.UserRoles
                               join role in db.Roles on userRole.RoleId equals role.Id
                               join rolePermission in db.RolePermissions on role.Id equals rolePermission.RoleId
                               where userRole.UserId == userId && userRole.IsActive && role.IsActive &&
                                     rolePermission.IsActive && rolePermission.PermissionId == permission.Id
                               select rolePermission.Id).AnyAsync(cancellationToken);

        return PermissionDecision.IsAllowed(inherited, mode);
    }
}
