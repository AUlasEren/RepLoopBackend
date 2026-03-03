using Microsoft.EntityFrameworkCore;
using SettingsService.Application.Common.Interfaces;
using SettingsService.Application.Features.Settings.Commands.UpdateNotificationSettings;
using SettingsService.Application.Features.Settings.Commands.UpdatePrivacySettings;
using SettingsService.Application.Features.Settings.Commands.UpdateWorkoutSettings;
using SettingsService.Application.Features.Settings.Common;
using SettingsService.Domain.Entities;

namespace SettingsService.Application.Features.Settings;

public class SettingsManager
{
    private readonly ISettingsDbContext _context;

    public SettingsManager(ISettingsDbContext context)
    {
        _context = context;
    }

    public async Task<SettingsDto> GetSettingsAsync(Guid userId, CancellationToken ct)
    {
        var settings = await _context.UserSettings
            .FirstOrDefaultAsync(s => s.UserId == userId, ct);

        if (settings is null)
        {
            settings = new UserSettings { UserId = userId };
            _context.UserSettings.Add(settings);
            await _context.SaveChangesAsync(ct);
        }

        return ToDto(settings);
    }

    public async Task<SettingsDto> UpdateWorkoutSettingsAsync(UpdateWorkoutSettingsCommand command, Guid userId, CancellationToken ct)
    {
        var settings = await GetOrCreateAsync(userId, ct);

        if (command.WeightUnit.HasValue) settings.WeightUnit = command.WeightUnit.Value;
        if (command.DistanceUnit.HasValue) settings.DistanceUnit = command.DistanceUnit.Value;
        if (command.DefaultDurationMinutes.HasValue) settings.DefaultDurationMinutes = command.DefaultDurationMinutes.Value;
        if (command.RestBetweenSetsSeconds.HasValue) settings.RestBetweenSetsSeconds = command.RestBetweenSetsSeconds.Value;
        if (command.WorkoutDays is not null) settings.WorkoutDays = string.Join(',', command.WorkoutDays);

        settings.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
        return ToDto(settings);
    }

    public async Task<SettingsDto> UpdateNotificationSettingsAsync(UpdateNotificationSettingsCommand command, Guid userId, CancellationToken ct)
    {
        var settings = await GetOrCreateAsync(userId, ct);

        if (command.EmailNotifications.HasValue) settings.EmailNotifications = command.EmailNotifications.Value;
        if (command.PushNotifications.HasValue) settings.PushNotifications = command.PushNotifications.Value;
        if (command.WorkoutReminders.HasValue) settings.WorkoutReminders = command.WorkoutReminders.Value;
        if (command.WeeklyReport.HasValue) settings.WeeklyReport = command.WeeklyReport.Value;
        if (command.AchievementAlerts.HasValue) settings.AchievementAlerts = command.AchievementAlerts.Value;

        settings.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
        return ToDto(settings);
    }

    public async Task<SettingsDto> UpdatePrivacySettingsAsync(UpdatePrivacySettingsCommand command, Guid userId, CancellationToken ct)
    {
        var settings = await GetOrCreateAsync(userId, ct);

        if (command.AllowDataAnalysis.HasValue) settings.AllowDataAnalysis = command.AllowDataAnalysis.Value;

        settings.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
        return ToDto(settings);
    }

    private async Task<UserSettings> GetOrCreateAsync(Guid userId, CancellationToken ct)
    {
        var settings = await _context.UserSettings
            .FirstOrDefaultAsync(s => s.UserId == userId, ct);

        if (settings is null)
        {
            settings = new UserSettings { UserId = userId };
            _context.UserSettings.Add(settings);
        }

        return settings;
    }

    internal static SettingsDto ToDto(UserSettings s) => new(
        new WorkoutSettingsDto(
            s.WeightUnit,
            s.DistanceUnit,
            s.DefaultDurationMinutes,
            s.RestBetweenSetsSeconds,
            s.WorkoutDays.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
        ),
        new NotificationSettingsDto(
            s.EmailNotifications,
            s.PushNotifications,
            s.WorkoutReminders,
            s.WeeklyReport,
            s.AchievementAlerts
        ),
        new PrivacySettingsDto(
            s.AllowDataAnalysis
        )
    );
}
