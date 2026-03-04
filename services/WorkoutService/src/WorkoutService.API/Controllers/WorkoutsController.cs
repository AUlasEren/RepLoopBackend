using MediatR;
using RepLoopBackend.SharedKernel.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutService.Application.Features.Workouts.Commands.CreateWorkout;
using WorkoutService.Application.Features.Workouts.Commands.DeleteWorkout;
using WorkoutService.Application.Features.Workouts.Commands.DuplicateWorkout;
using WorkoutService.Application.Features.Workouts.Commands.UpdateWorkout;
using WorkoutService.Application.Features.Workouts.Queries.GetWorkoutById;
using WorkoutService.Application.Features.Workouts.Queries.GetWorkoutHistory;
using WorkoutService.Application.Features.Workouts.Queries.GetWorkouts;

namespace WorkoutService.API.Controllers;

[Route("api/workouts")]
[Authorize]
public class WorkoutsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public WorkoutsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetWorkoutsQuery(page, pageSize));
        return Ok(result);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetWorkoutHistoryQuery(page, pageSize));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetWorkoutByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorkoutCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWorkoutCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("{id:guid}/duplicate")]
    public async Task<IActionResult> Duplicate(Guid id, [FromBody] DuplicateWorkoutRequest? request)
    {
        var newId = await _mediator.Send(new DuplicateWorkoutCommand { Id = id, Name = request?.Name });
        return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteWorkoutCommand(id));
        return NoContent();
    }
}

public record DuplicateWorkoutRequest(string? Name);
