using MediatR;

namespace RepLoopBackend.Application.Features.Auth.Commands.ResetPassword;

public record ResetPasswordCommand(string Email, string Token, string NewPassword) : IRequest;
