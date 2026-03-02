using FluentValidation;

namespace WorkoutService.Application.Features.Workouts.Commands.UpdateWorkout;

public class UpdateWorkoutCommandValidator : AbstractValidator<UpdateWorkoutCommand>
{
    public UpdateWorkoutCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
