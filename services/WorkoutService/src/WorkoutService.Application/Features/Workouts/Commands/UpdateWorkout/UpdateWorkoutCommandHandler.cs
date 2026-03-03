using MediatR;
using Microsoft.EntityFrameworkCore;
using RepLoopBackend.SharedKernel.Exceptions;
using WorkoutService.Application.Common.Interfaces;

namespace WorkoutService.Application.Features.Workouts.Commands.UpdateWorkout;

public class UpdateWorkoutCommandHandler : IRequestHandler<UpdateWorkoutCommand>
{
    private readonly IWorkoutDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public UpdateWorkoutCommandHandler(IWorkoutDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task Handle(UpdateWorkoutCommand request, CancellationToken ct)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        var workout = await _context.Workouts
            .FirstOrDefaultAsync(w => w.Id == request.Id && w.UserId == userId, ct)
            ?? throw new NotFoundException(ErrorCodes.WorkoutNotFound, "Workout", request.Id);

        workout.Name = request.Name;
        workout.Description = request.Description;
        workout.Notes = request.Notes;
        workout.ScheduledDate = request.ScheduledDate;
        workout.DurationMinutes = request.DurationMinutes;
        workout.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);
    }
}
