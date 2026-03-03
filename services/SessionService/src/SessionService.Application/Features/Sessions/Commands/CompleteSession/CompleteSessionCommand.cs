using MediatR;

namespace SessionService.Application.Features.Sessions.Commands.CompleteSession;

public record CompleteSessionCommand : IRequest
{
    public Guid Id { get; init; }
    public string? Notes { get; init; }
}
