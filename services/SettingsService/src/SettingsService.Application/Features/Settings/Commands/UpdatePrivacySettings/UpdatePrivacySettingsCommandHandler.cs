using MediatR;
using SettingsService.Application.Common.Interfaces;
using SettingsService.Application.Features.Settings.Common;

namespace SettingsService.Application.Features.Settings.Commands.UpdatePrivacySettings;

public class UpdatePrivacySettingsCommandHandler : IRequestHandler<UpdatePrivacySettingsCommand, SettingsDto>
{
    private readonly SettingsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public UpdatePrivacySettingsCommandHandler(SettingsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<SettingsDto> Handle(UpdatePrivacySettingsCommand request, CancellationToken ct)
        => _manager.UpdatePrivacySettingsAsync(request, _currentUser.UserId, ct);
}
