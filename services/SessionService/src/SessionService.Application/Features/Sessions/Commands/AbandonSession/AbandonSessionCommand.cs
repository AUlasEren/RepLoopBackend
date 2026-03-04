using MediatR;

namespace SessionService.Application.Features.Sessions.Commands.AbandonSession;

public record AbandonSessionCommand : IRequest
{
    public Guid Id { get; init; }
    public string? Notes { get; init; }
}