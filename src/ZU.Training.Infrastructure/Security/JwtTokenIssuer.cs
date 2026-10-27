using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ZU.Training.Application.Security;
using ZU.Training.Domain.Identity;

namespace ZU.Training.Infrastructure.Security;

public sealed class JwtTokenIssuer(string signingKey, TimeSpan normalLifetime, TimeSpan rememberedLifetime) : ITokenIssuer
{
    public const string Issuer = "zu-training-platform";
    public const string Audience = "zu-training-portal";
    public const string CookieName = "zu_access";

    public IssuedToken Issue(UserAccount user, IReadOnlyCollection<string> roles, bool rememberMe)
    {
        var now = DateTimeOffset.UtcNow;
        var expiry = now.Add(rememberMe ? rememberedLifetime : normalLifetime);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, user.LoginName),
            new("auth_version", user.AuthVersion.ToString()),
            new("password_change_required", (!user.FirstLoginCompleted && !user.IsExternalTrainer).ToString().ToLowerInvariant())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(Issuer, Audience, claims, now.UtcDateTime, expiry.UtcDateTime, credentials);
        return new IssuedToken(new JwtSecurityTokenHandler().WriteToken(jwt), expiry);
    }
}
