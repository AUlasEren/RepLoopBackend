using MediatR;
using StatisticsService.Application.Common.Interfaces;

namespace StatisticsService.Application.Features.Statistics.Commands.LogExercise;

public class LogExerciseCommandHandler : IRequestHandler<LogExerciseCommand, Guid>
{
    private readonly StatisticsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public LogExerciseCommandHandler(StatisticsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<Guid> Handle(LogExerciseCommand request, CancellationToken ct)
        => _manager.LogExerciseAsync(request, _currentUser.UserId, ct);
}
