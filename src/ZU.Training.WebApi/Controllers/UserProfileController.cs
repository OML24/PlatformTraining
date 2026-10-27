using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZU.Training.Application.Administration;

namespace ZU.Training.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public sealed class UserProfileController(UserProfileService profiles) : ControllerBase
{
    [HttpGet("me/profile")]
    public async Task<IActionResult> MyProfile(CancellationToken ct)
    {
        var userId = CurrentUserId();
        return Ok(await profiles.GetProfileAsync(userId, userId, ct));
    }

    [HttpGet("{id:guid}/profile")]
    public async Task<IActionResult> UserProfile(Guid id, CancellationToken ct)
    {
        var requesterId = CurrentUserId();
        try { return Ok(await profiles.GetProfileAsync(requesterId, id, ct)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    private Guid CurrentUserId()
        => Guid.TryParse(User.FindFirstValue("sub"), out var id) ? id : throw new UnauthorizedAccessException();
}
