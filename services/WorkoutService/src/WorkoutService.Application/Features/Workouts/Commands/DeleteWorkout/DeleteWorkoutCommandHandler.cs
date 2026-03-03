using MediatR;
using Microsoft.EntityFrameworkCore;
using RepLoopBackend.SharedKernel.Exceptions;
using WorkoutService.Application.Common.Interfaces;

namespace WorkoutService.Application.Features.Workouts.Commands.DeleteWorkout;

public class DeleteWorkoutCommandHandler : IRequestHandler<DeleteWorkoutCommand>
{
    private readonly IWorkoutDbContext _context;

    public DeleteWorkoutCommandHandler(IWorkoutDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteWorkoutCommand request, CancellationToken ct)
    {
        var workout = await _context.Workouts
            .FirstOrDefaultAsync(w => w.Id == request.Id && w.UserId == request.UserId, ct)
            ?? throw new NotFoundException(ErrorCodes.WorkoutNotFound, "Workout", request.Id);

        _context.Workouts.Remove(workout);
        await _context.SaveChangesAsync(ct);
    }
}
