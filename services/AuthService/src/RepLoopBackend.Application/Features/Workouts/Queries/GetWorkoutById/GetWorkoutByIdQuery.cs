using MediatR;

namespace RepLoopBackend.Application.Features.Workouts.Queries.GetWorkoutById;

public record GetWorkoutByIdQuery(Guid Id) : IRequest<WorkoutDto?>;
