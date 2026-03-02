using FluentValidation;

namespace WorkoutService.Application.Features.Workouts.Commands.CreateWorkout;

public class CreateWorkoutCommandValidator : AbstractValidator<CreateWorkoutCommand>
{
    public CreateWorkoutCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(200);
        RuleFor(x => x.DurationMinutes).GreaterThan(0).When(x => x.DurationMinutes.HasValue);
    }
}
