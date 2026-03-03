using MediatR;
using ExerciseService.Application.Common.Interfaces;

namespace ExerciseService.Application.Features.Exercises.Commands.DeleteExercise;

public class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand>
{
    private readonly ExercisesManager _manager;
    private readonly ICurrentUserService _currentUser;

    public DeleteExerciseCommandHandler(ExercisesManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task Handle(DeleteExerciseCommand request, CancellationToken ct)
        => _manager.DeleteExerciseAsync(request.Id, _currentUser.UserId, ct);
}
