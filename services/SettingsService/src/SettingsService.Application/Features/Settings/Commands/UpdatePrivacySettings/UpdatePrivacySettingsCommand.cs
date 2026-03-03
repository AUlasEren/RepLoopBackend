using MediatR;
using SettingsService.Application.Features.Settings.Common;

namespace SettingsService.Application.Features.Settings.Commands.UpdatePrivacySettings;

public record UpdatePrivacySettingsCommand(
    bool? AllowDataAnalysis
) : IRequest<SettingsDto>
{
    public Guid UserId { get; init; }
}
