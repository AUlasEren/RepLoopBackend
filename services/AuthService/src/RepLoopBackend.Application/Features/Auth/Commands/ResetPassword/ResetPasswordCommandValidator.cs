using FluentValidation;

namespace RepLoopBackend.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Code).NotEmpty().Length(6)
            .WithMessage("Kod 6 haneli olmalıdır.");
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(6);
    }
}
