using MediatR;
using SettingsService.Application.Features.Settings.Common;

namespace SettingsService.Application.Features.Settings.Commands.UpdateNotificationSettings;

public record UpdateNotificationSettingsCommand(
    bool? EmailNotifications,
    bool? PushNotifications,
    bool? WorkoutReminders,
    bool? WeeklyReport,
    bool? AchievementAlerts
) : IRequest<SettingsDto>
{
    public Guid UserId { get; init; }
}
