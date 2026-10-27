using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZU.Training.Application.Training;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/courses/historical")]
public sealed class HistoricalCoursesController(
    HistoricalCourseService historicalCourses,
    IConfiguration configuration) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateHistoricalCourse([FromBody] HistoricalCourseInput input, CancellationToken ct)
    {
        var portalBaseUrl = configuration["WebPortal:Origin"] ?? "https://portal.zu.edu.jo";
        try
        {
            var actorId = CurrentUserId();
            var result = await historicalCourses.CreateHistoricalCourseAsync(actorId, input, portalBaseUrl, ct);
            return Ok(result);
        }
        catch (UnauthorizedAccessException) { return Forbid(); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue("sub"), out var id) ? id : throw new UnauthorizedAccessException();
}
