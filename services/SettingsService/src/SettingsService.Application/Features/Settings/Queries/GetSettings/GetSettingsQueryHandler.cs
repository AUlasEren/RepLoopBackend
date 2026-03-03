using MediatR;
using SettingsService.Application.Common.Interfaces;
using SettingsService.Application.Features.Settings.Common;

namespace SettingsService.Application.Features.Settings.Queries.GetSettings;

public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, SettingsDto>
{
    private readonly SettingsManager _manager;
    private readonly ICurrentUserService _currentUser;

    public GetSettingsQueryHandler(SettingsManager manager, ICurrentUserService currentUser)
    {
        _manager = manager;
        _currentUser = currentUser;
    }

    public Task<SettingsDto> Handle(GetSettingsQuery request, CancellationToken ct)
        => _manager.GetSettingsAsync(_currentUser.UserId, ct);
}
