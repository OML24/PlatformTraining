using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZU.Training.Application.Integrations;
using ZU.Training.Domain.Identity;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Authorize(Policy = PermissionKeys.SyncUniversity)]
[Route("api/admin/university-sync")]
public sealed class UniversitySyncController(IUniversitySyncService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Refresh(CancellationToken ct)
    {
        if (!Guid.TryParse(User.FindFirstValue("sub"), out var actorId)) return Unauthorized();
        var result = await service.RefreshAsync(actorId, HttpContext.Connection.RemoteIpAddress?.ToString(), ct);
        return result.Error is null ? Ok(result) : StatusCode(StatusCodes.Status502BadGateway, result);
    }

    [HttpGet("runs")]
    public async Task<IActionResult> Runs(CancellationToken ct)
        => Ok(await service.RecentRunsAsync(ct));

    [HttpGet("missing-employees")]
    public async Task<IActionResult> MissingEmployees(CancellationToken ct)
        => Ok(await service.MissingEmployeesAsync(ct));
}
