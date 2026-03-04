using MediatR;
using RepLoopBackend.SharedKernel.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatisticsService.Application.Features.Statistics.Commands.AddBodyMeasurement;
using StatisticsService.Application.Features.Statistics.Commands.LogExercise;
using StatisticsService.Application.Features.Statistics.Queries.GetBodyMeasurements;
using StatisticsService.Application.Features.Statistics.Queries.GetPersonalRecords;
using StatisticsService.Application.Features.Statistics.Queries.GetStrengthProgress;

namespace StatisticsService.API.Controllers;

[Route("api/statistics")]
[Authorize]
public class StatisticsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public StatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("strength")]
    public async Task<IActionResult> GetStrengthProgress(
        [FromQuery] Guid exerciseId, [FromQuery] string period = "30d")
    {
        var result = await _mediator.Send(new GetStrengthProgressQuery(exerciseId, period));
        return Ok(result);
    }

    [HttpGet("personal-records")]
    public async Task<IActionResult> GetPersonalRecords()
    {
        var result = await _mediator.Send(new GetPersonalRecordsQuery());
        return Ok(result);
    }

    [HttpGet("body-measurements")]
    public async Task<IActionResult> GetBodyMeasurements(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetBodyMeasurementsQuery(page, pageSize));
        return Ok(result);
    }

    [HttpPost("body-measurements")]
    public async Task<IActionResult> AddBodyMeasurement([FromBody] AddBodyMeasurementCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBodyMeasurements), new { id });
    }

    [HttpPost("exercise-logs")]
    public async Task<IActionResult> LogExercise([FromBody] LogExerciseCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { id });
    }
}
