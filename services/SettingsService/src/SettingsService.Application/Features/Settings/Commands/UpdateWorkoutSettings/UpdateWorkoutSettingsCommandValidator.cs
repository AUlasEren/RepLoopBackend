using FluentValidation;

namespace SettingsService.Application.Features.Settings.Commands.UpdateWorkoutSettings;

public class UpdateWorkoutSettingsCommandValidator : AbstractValidator<UpdateWorkoutSettingsCommand>
{
    public UpdateWorkoutSettingsCommandValidator()
    {
        RuleFor(x => x.DefaultDurationMinutes)
            .GreaterThan(0).When(x => x.DefaultDurationMinutes.HasValue)
            .WithMessage("Varsayılan süre 0'dan büyük olmalıdır.");

        RuleFor(x => x.RestBetweenSetsSeconds)
            .GreaterThanOrEqualTo(0).When(x => x.RestBetweenSetsSeconds.HasValue)
            .WithMessage("Setler arası dinlenme süresi negatif olamaz.");
    }
}
