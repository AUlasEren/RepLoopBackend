using MediatR;

namespace UserService.Application.Features.Users.Commands.UpdateAvatar;

public record UpdateAvatarCommand(Stream FileStream, string ContentType, string FileName) : IRequest<string>;
