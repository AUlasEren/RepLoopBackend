using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using RepLoopBackend.Application.Common.Interfaces;

namespace RepLoopBackend.Infrastructure.Services;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly string? _clientId;

    public GoogleAuthService(IConfiguration configuration)
    {
        _clientId = configuration["GoogleSettings:ClientId"];
    }

    public async Task<GoogleUserInfo?> VerifyTokenAsync(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings();

            if (!string.IsNullOrEmpty(_clientId))
                settings.Audience = [_clientId];

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var name = payload.Name ?? payload.Email.Split('@')[0];

            return new GoogleUserInfo(payload.Email, name, payload.Picture);
        }
        catch
        {
            return null;
        }
    }
}