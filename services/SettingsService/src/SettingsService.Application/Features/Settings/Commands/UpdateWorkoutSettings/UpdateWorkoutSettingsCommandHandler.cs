using MediatR;
using SettingsService.Application.Common.Interfaces;
using SettingsService.Application.Features.Settings.Common;

namespace SettingsService.Application.Features.Settings.Commands.UpdateWorkoutSettings;

public class UpdateWorkoutSettingsCommandHandler : IRequestHandler<UpdateWorkoutSettingsCommand, SettingsDto>
{
    private readonly SettingsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public UpdateWorkoutSettingsCommandHandler(SettingsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<SettingsDto> Handle(UpdateWorkoutSettingsCommand request, CancellationToken ct)
        => _manager.UpdateWorkoutSettingsAsync(request, _currentUser.UserId, ct);
}
