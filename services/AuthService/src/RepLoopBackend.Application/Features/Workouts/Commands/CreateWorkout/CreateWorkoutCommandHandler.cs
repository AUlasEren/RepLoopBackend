using MediatR;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Domain.Entities;

namespace RepLoopBackend.Application.Features.Workouts.Commands.CreateWorkout;

public class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public CreateWorkoutCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateWorkoutCommand request, CancellationToken ct)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");

        var workout = new Workout
        {
            Name = request.Name,
            Description = request.Description,
            UserId = userId
        };

        _context.Workouts.Add(workout);
        await _context.SaveChangesAsync(ct);
        return workout.Id;
    }
}
