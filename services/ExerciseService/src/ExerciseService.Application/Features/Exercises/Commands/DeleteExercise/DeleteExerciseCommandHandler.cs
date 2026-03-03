using MediatR;
using ExerciseService.Application.Common.Interfaces;

namespace ExerciseService.Application.Features.Exercises.Commands.DeleteExercise;

public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand>
{
    private readonly ExercisesManager _manager;

    public DeleteExerciseCommandHandler(ExercisesManager manager)
    {
        _manager = manager;
    }

    public Task Handle(DeleteExerciseCommand request, CancellationToken ct)
        => _manager.DeleteExerciseAsync(request.Id, ct);
}
