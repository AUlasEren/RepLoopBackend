using MediatR;
using ExerciseService.Application.Common.Interfaces;

namespace ExerciseService.Application.Features.Exercises.Commands.UpdateExercise;

public class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand>
{
    private readonly ExercisesManager _manager;
    private readonly ICurrentUserService _currentUser;

    public UpdateExerciseCommandHandler(ExercisesManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task Handle(UpdateExerciseCommand request, CancellationToken ct)
        => _manager.UpdateExerciseAsync(request, _currentUser.UserId, ct);
}
