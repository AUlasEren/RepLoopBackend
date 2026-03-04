using FluentValidation;

namespace StatisticsService.Application.Features.Statistics.Commands.LogExercise;

public class LogExerciseCommandValidator : AbstractValidator<LogExerciseCommand>
{
    public LogExerciseCommandValidator()
    {
        RuleFor(x => x.ExerciseId).NotEmpty();
        RuleFor(x => x.ExerciseName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.WeightKg).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Reps).GreaterThan(0);
        RuleFor(x => x.PerformedAt).NotEmpty();
    }
}
