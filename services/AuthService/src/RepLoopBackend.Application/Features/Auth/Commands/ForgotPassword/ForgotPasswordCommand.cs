using MediatR;

namespace RepLoopBackend.Application.Features.Auth.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest;
