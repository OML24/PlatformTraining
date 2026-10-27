using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZU.Training.Application.Settings;
using ZU.Training.Domain.Identity;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Route("api/settings")]
[Authorize]
public sealed class SettingsController(SettingsService service) : ControllerBase
{
    [HttpGet("hints")]
    [AllowAnonymous]
    public async Task<IActionResult> Hints(CancellationToken cancellationToken)
    {
        var hints = await service.GetHintsAsync(cancellationToken);
        return Ok(hints.Select(x => new { x.Id, x.ScreenKey, x.FieldKey, x.TextAr, x.TextEn }));
    }

    [HttpPost("hints")]
    [Authorize(Policy = PermissionKeys.ManageHints)]
    public async Task<IActionResult> CreateHint([FromBody] FieldHintInput input, CancellationToken cancellationToken)
        => await SaveHint(null, input, cancellationToken);

    [HttpPut("hints/{id:guid}")]
    [Authorize(Policy = PermissionKeys.ManageHints)]
    public async Task<IActionResult> UpdateHint(Guid id, [FromBody] FieldHintInput input,
        CancellationToken cancellationToken)
        => await SaveHint(id, input, cancellationToken);

    [HttpDelete("hints/{id:guid}")]
    [Authorize(Policy = PermissionKeys.ManageHints)]
    public async Task<IActionResult> DeleteHint(Guid id, CancellationToken cancellationToken)
        => await service.DeleteHintAsync(id, cancellationToken) ? NoContent() : NotFound();

    private async Task<IActionResult> SaveHint(Guid? id, FieldHintInput input, CancellationToken cancellationToken)
    {
        try
        {
            var hint = await service.SaveHintAsync(id, input, cancellationToken);
            return Ok(new { hint.Id, hint.ScreenKey, hint.FieldKey, hint.TextAr, hint.TextEn });
        }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpGet("password-policy")]
    [AllowAnonymous]
    public async Task<IActionResult> PasswordPolicy(CancellationToken cancellationToken)
    {
        var policy = await service.GetPasswordPolicyAsync(cancellationToken);
        return policy is null ? NotFound() : Ok(new
        {
            policy.MinimumLength, policy.MaximumLength, policy.AllowSpaces,
            policy.RequireMixedCharacterClasses, policy.BlockCommonPasswords, policy.EffectiveAtUtc
        });
    }

    [HttpPut("password-policy")]
    [Authorize(Policy = PermissionKeys.ManagePasswordPolicy)]
    public async Task<IActionResult> UpdatePasswordPolicy([FromBody] PasswordPolicyInput input,
        CancellationToken cancellationToken)
    {
        try
        {
            var policy = await service.UpdatePasswordPolicyAsync(input, cancellationToken);
            return Ok(new { policy.MinimumLength, policy.MaximumLength, policy.AllowSpaces,
                policy.RequireMixedCharacterClasses, policy.BlockCommonPasswords, policy.EffectiveAtUtc });
        }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpGet("certificate-templates")]
    [Authorize(Policy = PermissionKeys.ManageCertificateTemplates)]
    public async Task<IActionResult> Templates(CancellationToken cancellationToken)
    {
        var templates = await service.GetTemplatesAsync(cancellationToken);
        return Ok(templates.Select(x => new { x.Id, x.NameAr, x.NameEn, x.Version, x.Sha256, x.CreatedAtUtc }));
    }

    [HttpPost("certificate-templates")]
    [Authorize(Policy = PermissionKeys.ManageCertificateTemplates)]
    [RequestSizeLimit(21 * 1024 * 1024)]
    public async Task<IActionResult> UploadTemplate([FromForm] string nameAr, [FromForm] string? nameEn,
        IFormFile? file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length is < 5 or > 20 * 1024 * 1024)
            return BadRequest(new { message = "A PDF of up to 20 MB is required." });
        try
        {
            await using var stream = file.OpenReadStream();
            var template = await service.UploadTemplateAsync(nameAr, nameEn, stream, cancellationToken);
            return Ok(new { template.Id, template.NameAr, template.NameEn, template.Version,
                template.Sha256, template.CreatedAtUtc });
        }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (InvalidDataException ex) { return BadRequest(new { message = ex.Message }); }
        catch (EndOfStreamException) { return BadRequest(new { message = "Invalid PDF." }); }
    }

    [HttpGet("certificate-templates/{id:guid}/file")]
    [Authorize(Policy = PermissionKeys.ManageCertificateTemplates)]
    public async Task<IActionResult> TemplateFile(Guid id, CancellationToken cancellationToken)
    {
        var template = (await service.GetTemplatesAsync(cancellationToken)).FirstOrDefault(x => x.Id == id);
        if (template is null) return NotFound();
        var stream = await service.OpenTemplateAsync(template.PdfStorageKey, cancellationToken);
        return stream is null ? NotFound() : File(stream, "application/pdf", $"certificate-template-v{template.Version}.pdf");
    }
}
