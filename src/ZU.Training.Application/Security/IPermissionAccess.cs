namespace ZU.Training.Application.Security;

public interface IPermissionAccess
{
    Task<bool> IsAllowedAsync(Guid userId, string permissionKey, CancellationToken cancellationToken);
}
