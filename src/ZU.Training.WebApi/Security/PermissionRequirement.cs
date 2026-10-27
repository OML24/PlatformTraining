using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ZU.Training.Application.Security;

namespace ZU.Training.WebApi.Security;

public sealed record PermissionRequirement(string Key) : IAuthorizationRequirement;

public sealed class PermissionHandler(IPermissionAccess access) : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (!Guid.TryParse(context.User.FindFirstValue("sub"), out var userId))
            return;
        if (await access.IsAllowedAsync(userId, requirement.Key, CancellationToken.None))
            context.Succeed(requirement);
    }
}
