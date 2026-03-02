using FluentValidation;

namespace UserService.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Age).InclusiveBetween(10, 120).When(x => x.Age.HasValue);
        RuleFor(x => x.HeightCm).InclusiveBetween(50, 300).When(x => x.HeightCm.HasValue);
        RuleFor(x => x.WeightKg).InclusiveBetween(20, 500).When(x => x.WeightKg.HasValue);
    }
}
