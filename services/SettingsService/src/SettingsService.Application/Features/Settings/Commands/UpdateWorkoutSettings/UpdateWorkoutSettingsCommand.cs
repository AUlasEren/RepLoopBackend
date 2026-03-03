using MediatR;
using SettingsService.Application.Features.Settings.Common;
using SettingsService.Domain.Enums;

namespace SettingsService.Application.Features.Settings.Commands.UpdateWorkoutSettings;

public record UpdateWorkoutSettingsCommand(
    WeightUnit? WeightUnit,
    DistanceUnit? DistanceUnit,
    int? DefaultDurationMinutes,
    int? RestBetweenSetsSeconds,
    List<string>? WorkoutDays
) : IRequest<SettingsDto>
{
    public Guid UserId { get; init; }
}
