using MediatR;
using Microsoft.EntityFrameworkCore;
using SettingsService.Application.Common.Interfaces;
using SettingsService.Application.Features.Settings.Common;
using SettingsService.Domain.Entities;

namespace SettingsService.Application.Features.Settings.Queries.GetSettings;

public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, SettingsDto>
{
    private readonly ISettingsDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetSettingsQueryHandler(ISettingsDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<SettingsDto> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Kullanıcı kimliği doğrulanamadı.");

        var settings = await _context.UserSettings
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

        if (settings is null)
        {
            settings = new UserSettings { UserId = userId };
            _context.UserSettings.Add(settings);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return ToDto(settings);
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
