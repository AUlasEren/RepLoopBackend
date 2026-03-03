using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Application.Features.Auth.Commands.ChangePassword;
using RepLoopBackend.SharedKernel.Exceptions;

namespace RepLoopBackend.Application.Features.Auth;

public class AuthManager
{
    private readonly IIdentityService _identityService;

    public AuthManager(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task ChangePasswordAsync(ChangePasswordCommand command, Guid userId, CancellationToken ct)
    {
        var (success, error) = await _identityService.ChangePasswordAsync(
            userId, command.CurrentPassword, command.NewPassword);

        if (!success)
            throw new BadRequestException(ErrorCodes.ChangePasswordFailed, error ?? "Şifre değiştirme başarısız.");
    }
}
