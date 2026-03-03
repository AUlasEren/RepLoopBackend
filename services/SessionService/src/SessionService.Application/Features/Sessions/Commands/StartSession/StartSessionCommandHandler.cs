using MediatR;
using SessionService.Application.Common.Interfaces;
using SessionService.Domain.Entities;

namespace SessionService.Application.Features.Sessions.Commands.StartSession;

public class StartSessionCommandHandler : IRequestHandler<StartSessionCommand, Guid>
{
    private readonly ISessionDbContext _context;

    public StartSessionCommandHandler(ISessionDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(StartSessionCommand request, CancellationToken ct)
    {
        var session = new WorkoutSession
        {
            UserId = request.UserId,
            WorkoutId = request.WorkoutId,
            WorkoutName = request.WorkoutName
        };

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync(ct);
        return session.Id;
    }
}
