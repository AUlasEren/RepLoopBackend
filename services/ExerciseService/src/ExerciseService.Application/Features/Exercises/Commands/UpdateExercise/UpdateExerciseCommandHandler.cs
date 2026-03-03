using MediatR;
using ExerciseService.Application.Common.Interfaces;

namespace ExerciseService.Application.Features.Exercises.Commands.UpdateExercise;

public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand>
{
    private readonly ExercisesManager _manager;

    public UpdateExerciseCommandHandler(ExercisesManager manager)
    {
        _manager = manager;
    }

    public Task Handle(UpdateExerciseCommand request, CancellationToken ct)
        => _manager.UpdateExerciseAsync(request, ct);
}
