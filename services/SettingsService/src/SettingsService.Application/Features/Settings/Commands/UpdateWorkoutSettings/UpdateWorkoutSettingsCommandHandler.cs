using MediatR;
using Microsoft.EntityFrameworkCore;
using SettingsService.Application.Common.Interfaces;
using SettingsService.Application.Features.Settings.Common;
using SettingsService.Application.Features.Settings.Queries.GetSettings;
using SettingsService.Domain.Entities;

namespace SettingsService.Application.Features.Settings.Commands.UpdateWorkoutSettings;

public class UpdateWorkoutSettingsCommandHandler : IRequestHandler<UpdateWorkoutSettingsCommand, SettingsDto>
{
    private readonly ISettingsDbContext _context;

    public UpdateWorkoutSettingsCommandHandler(ISettingsDbContext context)
    {
        _context = context;
    }

    public async Task<SettingsDto> Handle(UpdateWorkoutSettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await _context.UserSettings
            .FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);

        if (settings is null)
        {
            settings = new UserSettings { UserId = request.UserId };
            _context.UserSettings.Add(settings);
        }

        if (request.WeightUnit.HasValue)
            settings.WeightUnit = request.WeightUnit.Value;
        if (request.DistanceUnit.HasValue)
            settings.DistanceUnit = request.DistanceUnit.Value;
        if (request.DefaultDurationMinutes.HasValue)
            settings.DefaultDurationMinutes = request.DefaultDurationMinutes.Value;
        if (request.RestBetweenSetsSeconds.HasValue)
            settings.RestBetweenSetsSeconds = request.RestBetweenSetsSeconds.Value;
        if (request.WorkoutDays is not null)
            settings.WorkoutDays = string.Join(',', request.WorkoutDays);

        settings.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return GetSettingsQueryHandler.ToDto(settings);
    }
}
