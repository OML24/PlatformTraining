using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZU.Training.Application.Administration;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/analytics")]
public sealed class AnalyticsController(AnalyticsService analytics) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(CancellationToken ct)
    {
        var actorId = CurrentUserId();
        return Ok(await analytics.GetDashboardAnalyticsAsync(actorId, ct));
    }

    [HttpGet("courses/{courseId:guid}/sessions")]
    public async Task<IActionResult> GetCourseSessions(Guid courseId, CancellationToken ct)
    {
        try { return Ok(await analytics.GetCourseSessionsAnalyticsAsync(courseId, ct)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue("sub"), out var id) ? id : throw new UnauthorizedAccessException();
}
