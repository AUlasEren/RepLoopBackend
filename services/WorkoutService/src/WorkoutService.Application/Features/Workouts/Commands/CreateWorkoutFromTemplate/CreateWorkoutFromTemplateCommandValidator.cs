using FluentValidation;

namespace WorkoutService.Application.Features.Workouts.Commands.CreateWorkoutFromTemplate;

public class CreateWorkoutFromTemplateCommandValidator
    : AbstractValidator<CreateWorkoutFromTemplateCommand>
{
    public CreateWorkoutFromTemplateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Template adı zorunlu.")
            .MaximumLength(200);

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Süre 0'dan büyük olmalı.");

        RuleFor(x => x.Exercises)
            .NotEmpty().WithMessage("En az 1 egzersiz olmalı.");

        RuleForEach(x => x.Exercises).ChildRules(e =>
        {
            e.RuleFor(x => x.ExerciseId).NotEmpty();
            e.RuleFor(x => x.ExerciseName).NotEmpty();
            e.RuleFor(x => x.Order).GreaterThan(0);
            e.RuleFor(x => x.Sets).GreaterThan(0);
            e.RuleFor(x => x.Reps).GreaterThan(0);
        });
    }
}
