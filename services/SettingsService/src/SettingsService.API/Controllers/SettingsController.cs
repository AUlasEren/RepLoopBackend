using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SettingsService.Application.Features.Settings.Commands.UpdateNotificationSettings;
using SettingsService.Application.Features.Settings.Commands.UpdatePrivacySettings;
using SettingsService.Application.Features.Settings.Commands.UpdateWorkoutSettings;
using SettingsService.Application.Features.Settings.Queries.GetSettings;

namespace SettingsService.API.Controllers;

[ApiController]
[Route("api/settings")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SettingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetSettings()
    {
        var result = await _mediator.Send(new GetSettingsQuery());
        return Ok(result);
    }

    [HttpPatch("workout")]
    public async Task<IActionResult> UpdateWorkoutSettings([FromBody] UpdateWorkoutSettingsCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("notifications")]
    public async Task<IActionResult> UpdateNotificationSettings([FromBody] UpdateNotificationSettingsCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPatch("privacy")]
    public async Task<IActionResult> UpdatePrivacySettings([FromBody] UpdatePrivacySettingsCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
