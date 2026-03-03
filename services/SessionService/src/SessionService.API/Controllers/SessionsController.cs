using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SessionService.Application.Features.Sessions.Commands.CompleteSession;
using SessionService.Application.Features.Sessions.Commands.LogSet;
using SessionService.Application.Features.Sessions.Commands.StartSession;
using SessionService.Application.Features.Sessions.Commands.UpdateSession;
using SessionService.Application.Features.Sessions.Queries.GetSessionById;
using SessionService.Application.Features.Sessions.Queries.GetSessionHistory;

namespace SessionService.API.Controllers;

[Route("api/sessions")]
[Authorize]
public class SessionsController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public SessionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST /api/sessions — Yeni oturum başlat
    [HttpPost]
    public async Task<IActionResult> Start([FromBody] StartSessionCommand command)
    {
        var id = await _mediator.Send(command with { UserId = CurrentUserId });
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    // GET /api/sessions/history — Geçmiş antrenmanlar (sayfalı)
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _mediator.Send(new GetSessionHistoryQuery(CurrentUserId, page, pageSize));
        return Ok(result);
    }

    // GET /api/sessions/:id — Aktif oturum detayı
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetSessionByIdQuery(id, CurrentUserId));
        return result is null ? NotFound() : Ok(result);
    }

    // POST /api/sessions/:id/sets — Set kaydet
    [HttpPost("{id:guid}/sets")]
    public async Task<IActionResult> LogSet(Guid id, [FromBody] LogSetRequest request)
    {
        var command = new LogSetCommand
        {
            UserId = CurrentUserId,
            SessionId = id,
            ExerciseId = request.ExerciseId,
            ExerciseName = request.ExerciseName,
            SetNumber = request.SetNumber,
            Reps = request.Reps,
            WeightKg = request.WeightKg,
            DurationSeconds = request.DurationSeconds,
            Notes = request.Notes
        };
        var setId = await _mediator.Send(command);
        return Ok(new { id = setId });
    }

    // PATCH /api/sessions/:id — Duraklat veya devam et
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSessionRequest request)
    {
        await _mediator.Send(new UpdateSessionCommand { UserId = CurrentUserId, Id = id, Action = request.Action });
        return NoContent();
    }

    // POST /api/sessions/:id/complete — Antrenmanı bitir
    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteSessionRequest? request)
    {
        await _mediator.Send(new CompleteSessionCommand { UserId = CurrentUserId, Id = id, Notes = request?.Notes });
        return NoContent();
    }
}

public record LogSetRequest(
    Guid ExerciseId,
    string ExerciseName,
    int SetNumber,
    int? Reps,
    decimal? WeightKg,
    int? DurationSeconds,
    string? Notes);

public record UpdateSessionRequest(SessionAction Action);

public record CompleteSessionRequest(string? Notes);
