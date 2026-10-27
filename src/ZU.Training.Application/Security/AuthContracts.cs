using ZU.Training.Domain.Identity;
using ZU.Training.Domain.Settings;

namespace ZU.Training.Application.Security;

public sealed record IssuedToken(string Value, DateTimeOffset ExpiresAtUtc);
public sealed record LoginResult(IssuedToken Token, bool MustChangePassword, string DisplayName);

public interface ICredentialCipher
{
    string Encrypt(string plainText);
    string Decrypt(string encryptedText);
}

public interface ITokenIssuer
{
    IssuedToken Issue(UserAccount user, IReadOnlyCollection<string> roles, bool rememberMe);
}

public interface IAuthStore
{
    Task<UserAccount?> FindByLoginAsync(string loginName, CancellationToken cancellationToken);
    Task<UserAccount?> FindByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<string>> GetRoleKeysAsync(Guid userId, CancellationToken cancellationToken);
    Task<string?> GetSharedInitialPasswordAsync(CancellationToken cancellationToken);
    Task<PasswordPolicy?> GetCurrentPasswordPolicyAsync(CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
}
