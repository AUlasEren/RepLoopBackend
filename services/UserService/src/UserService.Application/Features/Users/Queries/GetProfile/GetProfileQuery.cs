using MediatR;
using UserService.Application.Features.Users.Common;

namespace UserService.Application.Features.Users.Queries.GetProfile;

public record GetProfileQuery(Guid UserId) : IRequest<UserProfileDto>;
