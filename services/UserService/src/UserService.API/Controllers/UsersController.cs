using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Users.Commands.DeleteAccount;
using UserService.Application.Features.Users.Commands.UpdateAvatar;
using UserService.Application.Features.Users.Commands.UpdateProfile;
using UserService.Application.Features.Users.Queries.GetProfile;

namespace UserService.API.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var result = await _mediator.Send(new GetProfileQuery());
        return Ok(result);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("avatar")]
    [RequestSizeLimit(5 * 1024 * 1024)] // 5 MB
    public async Task<IActionResult> UpdateAvatar(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest(new { error = "Dosya seçilmedi." });

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest(new { error = "Sadece JPEG, PNG ve WebP formatları desteklenir." });

        await using var stream = file.OpenReadStream();
        var avatarUrl = await _mediator.Send(new UpdateAvatarCommand(stream, file.ContentType, file.FileName));
        return Ok(new { avatarUrl });
    }

    [HttpDelete("account")]
    public async Task<IActionResult> DeleteAccount()
    {
        await _mediator.Send(new DeleteAccountCommand());
        return NoContent();
    }
}
