using FluentValidation;

namespace RepLoopBackend.Application.Features.Auth.Commands.AppleAuth;

public class AppleAuthCommandValidator : AbstractValidator<AppleAuthCommand>
{
    public AppleAuthCommandValidator()
    {
        RuleFor(x => x.IdentityToken).NotEmpty();
    }
}