using MediatR;
using ExerciseService.Application.Common.Interfaces;
using ExerciseService.Application.Features.Exercises.Common;

namespace ExerciseService.Application.Features.Exercises.Queries.GetExercises;

public class GetExercisesQueryHandler : IRequestHandler<GetExercisesQuery, ExerciseListDto>
{
    private readonly ExercisesManager _manager;

    public GetExercisesQueryHandler(ExercisesManager manager)
    {
        _manager = manager;
    }

    public Task<ExerciseListDto> Handle(GetExercisesQuery request, CancellationToken ct)
        => _manager.GetExercisesAsync(request.MuscleGroup, request.Equipment, request.Difficulty, request.Q, request.Page, request.PageSize, ct);
}
