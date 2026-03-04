using MediatR;
using RepLoopBackend.Application.Common.Models;

namespace RepLoopBackend.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResult>;