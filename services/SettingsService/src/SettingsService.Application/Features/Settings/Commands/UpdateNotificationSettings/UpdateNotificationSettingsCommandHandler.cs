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
    private readonly ICurrentUserService _currentUserService;

    public UpdateNotificationSettingsCommandHandler(ISettingsDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<SettingsDto> Handle(UpdateNotificationSettingsCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Kullanıcı kimliği doğrulanamadı.");

        var settings = await _context.UserSettings
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

        if (settings is null)
        {
            settings = new UserSettings { UserId = userId };
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
