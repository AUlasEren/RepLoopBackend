using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepLoopBackend.Application.Common.Interfaces;

namespace RepLoopBackend.Infrastructure.Services;

public class 
    AppleAuthService : IAppleAuthService
{
    private const string AppleKeysUrl = "https://appleid.apple.com/auth/keys";
    private const string AppleIssuer = "https://appleid.apple.com";

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string? _clientId;

    public AppleAuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _clientId = configuration["AppleSettings:ClientId"];
    }

    public async Task<AppleUserInfo?> VerifyTokenAsync(string identityToken, string? fullName = null)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var jwksJson = await client.GetStringAsync(AppleKeysUrl);
            var jwks = new JsonWebKeySet(jwksJson);

            var handler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = AppleIssuer,
                ValidateAudience = !string.IsNullOrEmpty(_clientId),
                ValidAudience = _clientId,
                IssuerSigningKeys = jwks.GetSigningKeys(),
                ValidateLifetime = true
            };

            var principal = handler.ValidateToken(identityToken, parameters, out _);
            var email = principal.FindFirstValue(ClaimTypes.Email)
                        ?? principal.FindFirstValue("email");

            if (string.IsNullOrEmpty(email))
                return null;

            var name = fullName ?? email.Split('@')[0];

            return new AppleUserInfo(email, name);
        }
        catch
        {
            return null;
        }
    }
}