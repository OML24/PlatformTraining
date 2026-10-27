using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZU.Training.Application.Training;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/impact")]
public sealed class ImpactController(ImpactService service) : ControllerBase
{
    [HttpGet("courses/{courseId:guid}/mine")]
    public Task<IActionResult> Mine(Guid courseId, CancellationToken ct)
        => Execute(() => service.MineAsync(ActorId(), courseId, ct));

    [HttpGet("courses/{courseId:guid}/roster")]
    public Task<IActionResult> Roster(Guid courseId, CancellationToken ct)
        => Execute(() => service.RosterAsync(ActorId(), courseId, ct));

    [HttpPost("courses/{courseId:guid}")]
    [RequestSizeLimit(21 * 1024 * 1024)]
    public async Task<IActionResult> Upload(Guid courseId, IFormFile? file, CancellationToken ct)
    {
        if (file is null || file.Length is < 5 or > 20 * 1024 * 1024)
            return BadRequest(new { message = "A PDF or DOCX up to 20 MB is required." });
        return await Execute(async () =>
        {
            await using var stream = file.OpenReadStream();
            var impact = await service.UploadAsync(ActorId(), courseId, stream, file.FileName, ct);
            return new { impact.Id, impact.Version, impact.Status };
        });
    }

    [HttpPut("{submissionId:guid}/review")]
    public Task<IActionResult> Review(Guid submissionId, [FromBody] ImpactReviewRequest request, CancellationToken ct)
        => Execute(async () =>
        {
            var result = await service.ReviewAsync(ActorId(), submissionId, request.Approve, request.NoteAr, ct);
            return new { result.Id, result.Status, result.ReviewNoteAr };
        });

    [HttpGet("{submissionId:guid}/file")]
    public async Task<IActionResult> File(Guid submissionId, CancellationToken ct)
    {
        try
        {
            var document = await service.DownloadAsync(ActorId(), submissionId, ct);
            var type = document.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)
                ? "application/pdf" : "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            return File(document.Content, type, document.FileName);
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    private Guid ActorId() => Guid.TryParse(User.FindFirstValue("sub"), out var id)
        ? id : throw new UnauthorizedAccessException();

    private async Task<IActionResult> Execute<T>(Func<Task<T>> action)
    {
        try { return Ok(await action()); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidDataException ex) { return BadRequest(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }
}

public sealed record ImpactReviewRequest(bool Approve, string? NoteAr);
