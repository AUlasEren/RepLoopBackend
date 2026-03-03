using MediatR;
using Microsoft.EntityFrameworkCore;
using SettingsService.Application.Common.Interfaces;
using SettingsService.Application.Features.Settings.Common;
using SettingsService.Application.Features.Settings.Queries.GetSettings;
using SettingsService.Domain.Entities;

namespace SettingsService.Application.Features.Settings.Commands.UpdatePrivacySettings;

public class UpdatePrivacySettingsCommandHandler : IRequestHandler<UpdatePrivacySettingsCommand, SettingsDto>
{
    private readonly ISettingsDbContext _context;

    public UpdatePrivacySettingsCommandHandler(ISettingsDbContext context)
    {
        _context = context;
    }

    public async Task<SettingsDto> Handle(UpdatePrivacySettingsCommand request, CancellationToken cancellationToken)
    {
        var settings = await _context.UserSettings
            .FirstOrDefaultAsync(s => s.UserId == request.UserId, cancellationToken);

        if (settings is null)
        {
            settings = new UserSettings { UserId = request.UserId };
            _context.UserSettings.Add(settings);
        }

        if (request.AllowDataAnalysis.HasValue)
            settings.AllowDataAnalysis = request.AllowDataAnalysis.Value;

        settings.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        return GetSettingsQueryHandler.ToDto(settings);
    }
}
