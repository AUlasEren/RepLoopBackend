using MediatR;
using SettingsService.Application.Features.Settings.Common;

namespace SettingsService.Application.Features.Settings.Queries.GetSettings;

public record GetSettingsQuery : IRequest<SettingsDto>;
