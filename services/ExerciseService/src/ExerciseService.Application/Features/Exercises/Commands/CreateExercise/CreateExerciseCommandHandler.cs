using MediatR;
using ExerciseService.Application.Common.Interfaces;

namespace ExerciseService.Application.Features.Exercises.Commands.CreateExercise;

public class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, Guid>
{
    private readonly ExercisesManager _manager;
    private readonly ICurrentUserService _currentUser;

    public CreateExerciseCommandHandler(ExercisesManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<Guid> Handle(CreateExerciseCommand request, CancellationToken ct)
        => _manager.CreateExerciseAsync(request, _currentUser.UserId, ct);
}
