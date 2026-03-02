using MediatR;
using RepLoopBackend.Application.Common.Interfaces;

namespace RepLoopBackend.Application.Features.Auth.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
{
    private readonly IIdentityService _identityService;
    private readonly ICurrentUserService _currentUserService;

    public ChangePasswordCommandHandler(IIdentityService identityService, ICurrentUserService currentUserService)
    {
        _identityService = identityService;
        _currentUserService = currentUserService;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Kullanıcı kimliği doğrulanamadı.");

        var (success, error) = await _identityService.ChangePasswordAsync(
            userId, request.CurrentPassword, request.NewPassword);

        if (!success)
            throw new InvalidOperationException(error ?? "Şifre değiştirme başarısız.");
    }
}
