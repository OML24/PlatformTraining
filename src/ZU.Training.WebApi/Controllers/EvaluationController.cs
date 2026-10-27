using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZU.Training.Application.Training;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/evaluation")]
public sealed class EvaluationController(EvaluationService service) : ControllerBase
{
    [HttpGet("form")]
    public Task<IActionResult> Latest(CancellationToken ct)
        => Execute(() => service.LatestAsync(ActorId(), ct));

    [HttpPost("form/publish")]
    public Task<IActionResult> Publish([FromBody] PublishEvaluationInput input, CancellationToken ct)
        => Execute(() => service.PublishAsync(ActorId(), input, ct));

    [HttpGet("courses/{courseId:guid}/form")]
    public Task<IActionResult> CourseForm(Guid courseId, CancellationToken ct)
        => Execute(() => service.CourseFormAsync(ActorId(), courseId, ct));

    [HttpPost("courses/{courseId:guid}/responses")]
    public Task<IActionResult> Submit(Guid courseId, [FromBody] SubmitEvaluationInput input, CancellationToken ct)
        => Execute(async () =>
        {
            var response = await service.SubmitAsync(ActorId(), courseId, input, ct);
            return new { response.Id, response.FormVersionId, response.SubmittedAtUtc };
        });

    [HttpGet("courses/{courseId:guid}/statistics")]
    public Task<IActionResult> Statistics(Guid courseId, [FromQuery] Guid? versionId, CancellationToken ct)
        => Execute(() => service.StatisticsAsync(ActorId(), courseId, versionId, ct));

    [HttpGet("versions")]
    public Task<IActionResult> Versions(CancellationToken ct)
        => Execute(() => service.VersionsAsync(ActorId(), ct));

    private Guid ActorId() => Guid.TryParse(User.FindFirstValue("sub"), out var id)
        ? id : throw new UnauthorizedAccessException();

    private async Task<IActionResult> Execute<T>(Func<Task<T>> action)
    {
        try { return Ok(await action()); }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }
}
