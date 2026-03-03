using MediatR;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.SharedKernel.Exceptions;

namespace RepLoopBackend.Application.Features.Auth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
{
    private readonly IIdentityService _identityService;

    public ResetPasswordCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var (success, error) = await _identityService.ResetPasswordAsync(
            request.Email, request.Token, request.NewPassword);

        if (!success)
            throw new BadRequestException(ErrorCodes.ResetPasswordFailed, error ?? "Şifre sıfırlama başarısız.");
    }
}
