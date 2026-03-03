using MediatR;
using Microsoft.EntityFrameworkCore;
using SessionService.Application.Common.Interfaces;
using SessionService.Application.Features.Sessions.Common;
using SessionService.Domain.Enums;

namespace SessionService.Application.Features.Sessions.Queries.GetSessionHistory;

public class GetSessionHistoryQueryHandler : IRequestHandler<GetSessionHistoryQuery, SessionHistoryDto>
{
    private readonly ISessionDbContext _context;

    public GetSessionHistoryQueryHandler(ISessionDbContext context)
    {
        _context = context;
    }

    public async Task<SessionHistoryDto> Handle(GetSessionHistoryQuery request, CancellationToken ct)
    {
        var query = _context.Sessions
            .Include(s => s.Sets)
            .Where(s => s.UserId == request.UserId && s.Status == SessionStatus.Completed)
            .OrderByDescending(s => s.CompletedAt);

        var totalCount = await query.CountAsync(ct);

        var sessions = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return new SessionHistoryDto
        {
            Items = sessions.Select(s => s.ToDto()).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
