using MediatR;
using ExerciseService.Application.Common.Interfaces;
using ExerciseService.Application.Features.Exercises.Common;

namespace ExerciseService.Application.Features.Exercises.Queries.GetExercises;

public class GetExercisesQueryHandler : IRequestHandler<GetExercisesQuery, List<ExerciseDto>>
{
    private readonly ExercisesManager _manager;

    public GetExercisesQueryHandler(ExercisesManager manager)
    {
        _manager = manager;
    }

    public Task<List<ExerciseDto>> Handle(GetExercisesQuery request, CancellationToken ct)
        => _manager.GetExercisesAsync(request.MuscleGroup, request.Equipment, request.Difficulty, ct);
}
