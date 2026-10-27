using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZU.Training.Application.Administration;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/admin/courses")]
public sealed class AdminCoursesController(CourseAdminService service) : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> List(CancellationToken ct)
        => Execute(() => service.ListAsync(ActorId(), ct));

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> Deactivate(Guid id, [FromBody] DeactivateCourseRequest request, CancellationToken ct)
        => Execute(() => service.DeactivateAsync(ActorId(), id,
            request.HideCertificates, request.WarningAcknowledged, ct));

    [HttpPost("{id:guid}/restore")]
    public Task<IActionResult> Restore(Guid id, CancellationToken ct)
        => Execute(() => service.RestoreAsync(ActorId(), id, ct));

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

public sealed record DeactivateCourseRequest(bool HideCertificates, bool WarningAcknowledged);
