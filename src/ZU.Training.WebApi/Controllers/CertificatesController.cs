using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZU.Training.Application.Training;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Route("api/certificates")]
public sealed class CertificatesController(CertificateVerificationService verification) : ControllerBase
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
}
