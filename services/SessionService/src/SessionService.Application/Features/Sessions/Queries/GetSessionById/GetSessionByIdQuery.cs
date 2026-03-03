using MediatR;
using SessionService.Application.Features.Sessions.Common;

namespace SessionService.Application.Features.Sessions.Queries.GetSessionById;

public record GetSessionByIdQuery(Guid Id) : IRequest<SessionDto?>;
