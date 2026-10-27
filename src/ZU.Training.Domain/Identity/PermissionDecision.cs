namespace ZU.Training.Domain.Identity;

public static class PermissionDecision
{
    public static bool IsAllowed(bool inheritedFromRole, PermissionOverrideMode overrideMode)
        => overrideMode switch
        {
            PermissionOverrideMode.Allow => true,
            PermissionOverrideMode.Deny => false,
            _ => inheritedFromRole
        };
}
