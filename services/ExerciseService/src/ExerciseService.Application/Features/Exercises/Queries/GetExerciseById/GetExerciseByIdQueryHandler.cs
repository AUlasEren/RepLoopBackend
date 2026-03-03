using MediatR;
using ExerciseService.Application.Common.Interfaces;
using ExerciseService.Application.Features.Exercises.Common;

namespace ExerciseService.Application.Features.Exercises.Queries.GetExerciseById;

public class GetExerciseByIdQueryHandler : IRequestHandler<GetExerciseByIdQuery, ExerciseDto?>
{
    private readonly ExercisesManager _manager;

    public GetExerciseByIdQueryHandler(ExercisesManager manager)
    {
        _manager = manager;
    }

    public Task<ExerciseDto?> Handle(GetExerciseByIdQuery request, CancellationToken ct)
        => _manager.GetExerciseByIdAsync(request.Id, ct);
}
