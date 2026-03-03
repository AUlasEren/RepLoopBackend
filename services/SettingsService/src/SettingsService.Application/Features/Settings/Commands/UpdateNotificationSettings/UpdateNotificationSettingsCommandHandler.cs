using MediatR;
using SettingsService.Application.Common.Interfaces;
using SettingsService.Application.Features.Settings.Common;

namespace SettingsService.Application.Features.Settings.Commands.UpdateNotificationSettings;

public class UpdateNotificationSettingsCommandHandler : IRequestHandler<UpdateNotificationSettingsCommand, SettingsDto>
{
    private readonly SettingsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public UpdateNotificationSettingsCommandHandler(SettingsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<SettingsDto> Handle(UpdateNotificationSettingsCommand request, CancellationToken ct)
        => _manager.UpdateNotificationSettingsAsync(request, _currentUser.UserId, ct);
}
