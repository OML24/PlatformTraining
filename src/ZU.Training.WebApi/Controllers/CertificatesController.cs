using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZU.Training.Application.Training;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Route("api/certificates")]
public sealed class CertificatesController(
    CertificateVerificationService verification,
    CertificateService certificateService,
    IConfiguration configuration) : ControllerBase
{
    [HttpGet("verify")]
    [AllowAnonymous]
    public async Task<IActionResult> Verify([FromQuery] string? cert, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(cert)) return BadRequest(new { message = "Certificate reference is required." });
        try { return Ok(await verification.VerifyAsync(cert, ct)); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> MyCertificates(CancellationToken ct)
    {
        var userId = CurrentUserId();
        return Ok(await certificateService.GetMyCertificatesAsync(userId, ct));
    }

    [HttpGet("user/{userId:guid}")]
    [Authorize]
    public async Task<IActionResult> UserCertificates(Guid userId, CancellationToken ct)
    {
        var requesterId = CurrentUserId();
        return Ok(await certificateService.GetUserPublicCertificatesAsync(requesterId, userId, ct));
    }

    [HttpGet("{id:guid}/download")]
    [Authorize]
    public async Task<IActionResult> Download(Guid id, CancellationToken ct)
    {
        try
        {
            var requesterId = CurrentUserId();
            var (stream, fileName) = await certificateService.DownloadAsync(id, requesterId, ct);
            return File(stream, "application/pdf", fileName);
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (FileNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPost("courses/{courseId:guid}/reevaluate")]
    [Authorize]
    public async Task<IActionResult> Reevaluate(Guid courseId, CancellationToken ct)
    {
        var portalBaseUrl = configuration["WebPortal:Origin"] ?? "https://portal.zu.edu.jo";
        await certificateService.ReevaluateCourseCertificatesAsync(courseId, portalBaseUrl, ct);
        return Ok(new { success = true });
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue("sub"), out var id) ? id : throw new UnauthorizedAccessException();
}
