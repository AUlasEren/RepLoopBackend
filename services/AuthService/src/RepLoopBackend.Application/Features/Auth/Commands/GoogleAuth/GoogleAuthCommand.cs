using MediatR;
using RepLoopBackend.Application.Common.Models;

namespace RepLoopBackend.Application.Features.Auth.Commands.GoogleAuth;

public record GoogleAuthCommand(string IdToken) : IRequest<AuthResult>;