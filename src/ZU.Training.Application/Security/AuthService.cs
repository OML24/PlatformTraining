using System.Security.Cryptography;
using System.Text;
using ZU.Training.Domain.Identity;

namespace ZU.Training.Application.Security;

public sealed class AuthService(IAuthStore store, ICredentialCipher cipher, ITokenIssuer tokenIssuer)
{
    public async Task<LoginResult?> LoginAsync(string loginName, string password, bool rememberMe, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(loginName) || string.IsNullOrEmpty(password))
            return null;

        var user = await store.FindByLoginAsync(loginName.Trim(), cancellationToken);
        if (user is null || !CanSignIn(user) || !await PasswordMatchesAsync(user, password, cancellationToken))
            return null;

        var roles = await store.GetRoleKeysAsync(user.Id, cancellationToken);
        return new LoginResult(tokenIssuer.Issue(user, roles, rememberMe), !user.FirstLoginCompleted && !user.IsExternalTrainer, user.NameAr);
    }

    public async Task<LoginResult?> ChangePasswordAsync(
        Guid userId, string oldPassword, string newPassword, bool rememberMe, CancellationToken cancellationToken)
    {
        var user = await store.FindByIdAsync(userId, cancellationToken);
        if (user is null || !CanSignIn(user) || !await PasswordMatchesAsync(user, oldPassword, cancellationToken))
            return null;

        var policy = await store.GetCurrentPasswordPolicyAsync(cancellationToken);
        if (policy is null || !IsAllowedByPolicy(newPassword, user.LoginName, policy))
            throw new ArgumentException("The new password does not meet the current policy.", nameof(newPassword));

        user.EncryptedCurrentPassword = cipher.Encrypt(newPassword);
        user.FirstLoginCompleted = true;
        user.AuthVersion++;
        await store.SaveChangesAsync(cancellationToken);

        var roles = await store.GetRoleKeysAsync(user.Id, cancellationToken);
        return new LoginResult(tokenIssuer.Issue(user, roles, rememberMe), false, user.NameAr);
    }

    public static bool CanSignIn(UserAccount user)
        => user.IsActive && (user.IsExternalTrainer || user.SourceEmploymentStatusCode == 13 || user.AccessOverrideAllow);

    private async Task<bool> PasswordMatchesAsync(UserAccount user, string password, CancellationToken cancellationToken)
    {
        if (SameSecret(password, cipher.Decrypt(user.EncryptedCurrentPassword)))
            return true;
        if (user.FirstLoginCompleted || user.IsExternalTrainer)
            return false;

        var shared = await store.GetSharedInitialPasswordAsync(cancellationToken);
        return shared is not null && SameSecret(password, cipher.Decrypt(shared));
    }

    private static bool SameSecret(string input, string expected)
    {
        var a = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        var b = SHA256.HashData(Encoding.UTF8.GetBytes(expected));
        return CryptographicOperations.FixedTimeEquals(a, b);
    }

    public static bool IsAllowedByPolicy(string password, string loginName, Domain.Settings.PasswordPolicy policy)
    {
        if (string.IsNullOrEmpty(password)) return false;
        if (password.Length < policy.MinimumLength || password.Length > policy.MaximumLength)
            return false;
        if (!policy.AllowSpaces && password.Contains(' '))
            return false;
        if (password.Contains(loginName, StringComparison.OrdinalIgnoreCase))
            return false;
        if (policy.RequireMixedCharacterClasses &&
            !(password.Any(char.IsLetter) && password.Any(char.IsDigit)))
            return false;
        if (policy.BlockCommonPasswords &&
            new[] { "password123456", "123456789012345", "qwerty123456789" }
                .Contains(password, StringComparer.OrdinalIgnoreCase))
            return false;
        return true;
    }

    public async Task<bool> ForgotPasswordAsync(string loginName, string email, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(loginName) || string.IsNullOrWhiteSpace(email)) return false;
        var user = await store.FindByLoginAsync(loginName.Trim(), ct);
        if (user is null || !user.IsActive || user.IsExternalTrainer) return false;
        if (string.IsNullOrWhiteSpace(user.Email) ||
            !string.Equals(user.Email.Trim(), email.Trim(), StringComparison.OrdinalIgnoreCase))
            return false;

        // In production environment sends current password to the verified university email
        return true;
    }
}
