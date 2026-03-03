using MediatR;
using Microsoft.EntityFrameworkCore;
using SettingsService.Application.Common.Interfaces;
using SettingsService.Application.Features.Settings.Common;
using SettingsService.Application.Features.Settings.Queries.GetSettings;
using SettingsService.Domain.Entities;

namespace SettingsService.Application.Features.Settings.Commands.UpdateNotificationSettings;

public class UpdateNotificationSettingsCommandHandler : IRequestHandler<UpdateNotificationSettingsCommand, SettingsDto>
{
    private readonly ISettingsDbContext _context;

    public UpdateNotificationSettingsCommandHandler(ISettingsDbContext context)
    {
        _context = context;
    }

    public async Task<SettingsDto> Handle(UpdateNotificationSettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await _context.UserSettings
            .FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);

        if (settings is null)
        {
            settings = new UserSettings { UserId = request.UserId };
            _context.UserSettings.Add(settings);
        }

        if (request.EmailNotifications.HasValue)
            settings.EmailNotifications = request.EmailNotifications.Value;
        if (request.PushNotifications.HasValue)
            settings.PushNotifications = request.PushNotifications.Value;
        if (request.WorkoutReminders.HasValue)
            settings.WorkoutReminders = request.WorkoutReminders.Value;
        if (request.WeeklyReport.HasValue)
            settings.WeeklyReport = request.WeeklyReport.Value;
        if (request.AchievementAlerts.HasValue)
            settings.AchievementAlerts = request.AchievementAlerts.Value;

        settings.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return GetSettingsQueryHandler.ToDto(settings);
    }
}
