using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ZU.Training.Application.Security;
using ZU.Training.Infrastructure.Security;
using ZU.Training.Domain.Identity;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(AuthService authService, IAuthStore store, IPermissionAccess permissionAccess) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request.LoginName, request.Password, request.RememberMe, cancellationToken);
        if (result is null)
            return Unauthorized(new { message = "بيانات الدخول غير صحيحة أو الحساب غير متاح." });

        WriteCookie(result.Token);
        return Ok(new { result.DisplayName, result.MustChangePassword, result.Token.ExpiresAtUtc });
    }

    [HttpPost("change-password")]
    [Authorize]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue("sub"), out var userId))
            return Unauthorized();

        try
        {
            var result = await authService.ChangePasswordAsync(
                userId, request.OldPassword, request.NewPassword, request.RememberMe, cancellationToken);
            if (result is null)
                return Unauthorized(new { message = "كلمة المرور الحالية غير صحيحة." });

            WriteCookie(result.Token);
            return Ok(new { result.DisplayName, result.MustChangePassword, result.Token.ExpiresAtUtc });
        }
        catch (ArgumentException)
        {
            return BadRequest(new { message = "كلمة المرور الجديدة لا توافق السياسة الحالية." });
        }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue("sub"), out var userId))
            return Unauthorized();
        var account = await store.FindByIdAsync(userId, cancellationToken);
        if (account is null)
            return Unauthorized();
        var roles = await store.GetRoleKeysAsync(userId, cancellationToken);
        var permissions = new List<string>();
        foreach (var key in PermissionKeys.All)
            if (await permissionAccess.IsAllowedAsync(userId, key, cancellationToken)) permissions.Add(key);
        return Ok(new
        {
            account.Id,
            account.LoginName,
            account.NameAr,
            account.NameEn,
            account.FacultyId,
            account.PreferredLanguage,
            Roles = roles,
            Permissions = permissions,
            MustChangePassword = !account.FirstLoginCompleted && !account.IsExternalTrainer
        });
    }

    [HttpPost("logout")]
    [AllowAnonymous]
    public IActionResult Logout()
    {
        var isDev = HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment();
        var deleteOptionsApi = new CookieOptions
        {
            Path = "/api",
            SameSite = SameSiteMode.Lax,
            Secure = !isDev,
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddDays(-1)
        };
        Response.Cookies.Delete(JwtTokenIssuer.CookieName, deleteOptionsApi);

        var deleteOptionsRoot = new CookieOptions
        {
            Path = "/",
            SameSite = SameSiteMode.Lax,
            Secure = !isDev,
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddDays(-1)
        };
        Response.Cookies.Delete(JwtTokenIssuer.CookieName, deleteOptionsRoot);

        return NoContent();
    }

    [HttpPut("language")]
    [Authorize]
    public async Task<IActionResult> Language([FromBody] LanguageRequest request, CancellationToken cancellationToken)
    {
        if (request.Language is not ("ar" or "en"))
            return BadRequest(new { message = "Unsupported language." });
        if (!Guid.TryParse(User.FindFirstValue("sub"), out var userId))
            return Unauthorized();
        var account = await store.FindByIdAsync(userId, cancellationToken);
        if (account is null) return Unauthorized();
        account.PreferredLanguage = request.Language;
        await store.SaveChangesAsync(cancellationToken);
        return Ok(new { account.PreferredLanguage });
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    [EnableRateLimiting("auth")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var ok = await authService.ForgotPasswordAsync(request.LoginName, request.Email, cancellationToken);
        if (!ok)
            return BadRequest(new { message = "الرقم الوظيفي أو البريد الإلكتروني غير متطابقين." });

        return Ok(new { message = "تم إرسال كلمة المرور إلى بريدك الإلكتروني الجامعي المسجل بنجاح." });
    }

    private void WriteCookie(IssuedToken token)
    {
        Response.Cookies.Append(JwtTokenIssuer.CookieName, token.Value, new CookieOptions
        {
            HttpOnly = true,
            Secure = !HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment(),
            SameSite = SameSiteMode.Lax,
            Path = "/api",
            Expires = token.ExpiresAtUtc
        });
    }
}

public sealed record LoginRequest(string LoginName, string Password, bool RememberMe);
public sealed record ChangePasswordRequest(string OldPassword, string NewPassword, bool RememberMe);
public sealed record LanguageRequest(string Language);
public sealed record ForgotPasswordRequest(string LoginName, string Email);
