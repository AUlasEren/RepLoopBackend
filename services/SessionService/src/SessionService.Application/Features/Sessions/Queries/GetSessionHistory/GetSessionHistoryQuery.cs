using MediatR;
using SessionService.Application.Features.Sessions.Common;

namespace SessionService.Application.Features.Sessions.Queries.GetSessionHistory;

public record GetSessionHistoryQuery(int Page = 1, int PageSize = 10) : IRequest<SessionHistoryDto>;
