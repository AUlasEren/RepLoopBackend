using MediatR;
using RepLoopBackend.Application.Features.Workouts.Queries.GetWorkoutById;

namespace RepLoopBackend.Application.Features.Workouts.Queries.GetAllWorkouts;

public record GetAllWorkoutsQuery : IRequest<List<WorkoutDto>>;
