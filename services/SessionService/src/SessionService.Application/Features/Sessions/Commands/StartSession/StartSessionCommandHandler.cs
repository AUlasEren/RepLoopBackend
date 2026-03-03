using MediatR;
using SessionService.Application.Common.Interfaces;
using SessionService.Domain.Entities;

namespace SessionService.Application.Features.Sessions.Commands.StartSession;

public class StartSessionCommandHandler : IRequestHandler<StartSessionCommand, Guid>
{
    private readonly ISessionDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public StartSessionCommandHandler(ISessionDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(StartSessionCommand request, CancellationToken ct)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var session = new WorkoutSession
        {
            UserId = userId,
            WorkoutId = request.WorkoutId,
            WorkoutName = request.WorkoutName
        };

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync(ct);
        return session.Id;
    }
}
