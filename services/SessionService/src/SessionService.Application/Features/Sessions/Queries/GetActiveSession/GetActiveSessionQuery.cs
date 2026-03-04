using MediatR;
using SessionService.Application.Features.Sessions.Common;

namespace SessionService.Application.Features.Sessions.Queries.GetActiveSession;

public record GetActiveSessionQuery : IRequest<SessionDto?>;
