using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZU.Training.Application.Administration;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/vault")]
public sealed class DocumentVaultController(DocumentVaultService vaultService) : ControllerBase
{
    [HttpGet("documents")]
    public async Task<IActionResult> ListDocuments(CancellationToken ct)
    {
        try
        {
            var actorId = CurrentUserId();
            return Ok(await vaultService.ListDocumentsAsync(actorId, ct));
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }

    [HttpGet("documents/{id:guid}/download")]
    public async Task<IActionResult> DownloadDocument(Guid id, CancellationToken ct)
    {
        try
        {
            var actorId = CurrentUserId();
            var (stream, fileName) = await vaultService.DownloadDocumentAsync(actorId, id, ct);
            return File(stream, "application/octet-stream", fileName);
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (FileNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    [HttpGet("audit-logs")]
    public async Task<IActionResult> ListAuditLogs([FromQuery] int take = 100, CancellationToken ct = default)
    {
        try
        {
            var actorId = CurrentUserId();
            return Ok(await vaultService.ListAuditLogsAsync(actorId, take, ct));
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue("sub"), out var id) ? id : throw new UnauthorizedAccessException();
}
