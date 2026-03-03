using MediatR;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.SharedKernel.Exceptions;

namespace RepLoopBackend.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly IIdentityService _identityService;

    public ChangePasswordCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var (success, error) = await _identityService.ChangePasswordAsync(
            request.UserId, request.CurrentPassword, request.NewPassword);

        if (!success)
            throw new BadRequestException(ErrorCodes.ChangePasswordFailed, error ?? "Şifre değiştirme başarısız.");
    }
}
