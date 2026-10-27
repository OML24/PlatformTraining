using Microsoft.EntityFrameworkCore;
using ZU.Training.Application.Security;
using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Settings;
using ZU.Training.Infrastructure.Data;

namespace ZU.Training.Infrastructure.Repositories;

public sealed class EfAuthStore(TrainingDbContext db) : IAuthStore
{
    public Task<UserAccount?> FindByLoginAsync(string loginName, CancellationToken cancellationToken)
        => db.UserAccounts.SingleOrDefaultAsync(x => x.LoginName == loginName, cancellationToken);

    public Task<UserAccount?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
        => db.UserAccounts.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<string>> GetRoleKeysAsync(Guid userId, CancellationToken cancellationToken)
        => await (from userRole in db.UserRoles
                  join role in db.Roles on userRole.RoleId equals role.Id
                  where userRole.UserId == userId && userRole.IsActive && role.IsActive
                  select role.Key).ToArrayAsync(cancellationToken);

    public async Task<string?> GetSharedInitialPasswordAsync(CancellationToken cancellationToken)
        => await db.SharedInitialPasswords
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => x.EncryptedPassword)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<PasswordPolicy?> GetCurrentPasswordPolicyAsync(CancellationToken cancellationToken)
        => db.PasswordPolicies.Where(x => x.IsActive)
            .OrderByDescending(x => x.EffectiveAtUtc)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await db.SaveChangesAsync(cancellationToken);
}
