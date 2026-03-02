using MediatR;
using RepLoopBackend.Application.Common.Models;

namespace RepLoopBackend.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<AuthResult>;
