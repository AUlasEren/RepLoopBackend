using FluentValidation;

namespace SessionService.Application.Features.Sessions.Commands.LogSet;

public class LogSetCommandValidator : AbstractValidator<LogSetCommand>
{
    public LogSetCommandValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty();
        RuleFor(x => x.ExerciseId).NotEmpty();
        RuleFor(x => x.ExerciseName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SetNumber).GreaterThan(0);
        RuleFor(x => x.Reps).GreaterThan(0).When(x => x.Reps.HasValue);
        RuleFor(x => x.WeightKg).GreaterThanOrEqualTo(0).When(x => x.WeightKg.HasValue);
        RuleFor(x => x.DurationSeconds).GreaterThan(0).When(x => x.DurationSeconds.HasValue);
    }
}
