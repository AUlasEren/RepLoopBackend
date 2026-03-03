using MediatR;
using Microsoft.EntityFrameworkCore;
using SessionService.Application.Common.Interfaces;
using SessionService.Application.Features.Sessions.Common;

namespace SessionService.Application.Features.Sessions.Queries.GetSessionById;

public class GetSessionByIdQueryHandler : IRequestHandler<GetSessionByIdQuery, SessionDto?>
{
    private readonly ISessionDbContext _context;

    public GetSessionByIdQueryHandler(ISessionDbContext context)
    {
        _context = context;
    }

    public async Task<SessionDto?> Handle(GetSessionByIdQuery request, CancellationToken ct)
    {
        var session = await _context.Sessions
            .Include(s => s.Sets)
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == request.UserId, ct);

        return session?.ToDto();
    }
}
