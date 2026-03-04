using MediatR;
using RepLoopBackend.SharedKernel.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExerciseService.Application.Features.Exercises.Commands.CreateExercise;
using ExerciseService.Application.Features.Exercises.Commands.DeleteExercise;
using ExerciseService.Application.Features.Exercises.Commands.UpdateExercise;
using ExerciseService.Application.Features.Exercises.Queries.GetExerciseById;
using ExerciseService.Application.Features.Exercises.Queries.GetExercises;

namespace ExerciseService.API.Controllers;

[Route("api/exercises")]
public class ExercisesController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public ExercisesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? muscleGroup,
        [FromQuery] string? equipment,
        [FromQuery] string? difficulty)
    {
        var result = await _mediator.Send(new GetExercisesQuery(muscleGroup, equipment, difficulty));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetExerciseByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateExerciseCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateExerciseCommand command)
    {
        if (id != command.Id) return BadRequest("ID mismatch.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteExerciseCommand(id));
        return NoContent();
    }
}
