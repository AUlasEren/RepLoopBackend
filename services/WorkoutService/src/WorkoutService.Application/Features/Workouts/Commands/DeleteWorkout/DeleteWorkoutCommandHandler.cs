using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkoutService.Application.Common.Interfaces;

namespace WorkoutService.Application.Features.Workouts.Commands.DeleteWorkout;

public class DeleteWorkoutCommandHandler : IRequestHandler<DeleteWorkoutCommand>
{
    private readonly IWorkoutDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public DeleteWorkoutCommandHandler(IWorkoutDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteWorkoutCommand request, CancellationToken ct)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var workout = await _context.Workouts
            .FirstOrDefaultAsync(w => w.Id == request.Id && w.UserId == userId, ct)
            ?? throw new KeyNotFoundException($"Workout {request.Id} not found.");

        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync(ct);
    }
}
