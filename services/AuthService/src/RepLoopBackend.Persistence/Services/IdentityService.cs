using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Application.Common.Models;
using RepLoopBackend.Persistence.Entities;

namespace RepLoopBackend.Persistence.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    public async Task<(bool Success, string? Error, UserInfo? User)> RegisterAsync(
        string email, string password, string displayName)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = displayName,
            IsProfileComplete = true
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            var error = string.Join(", ", result.Errors.Select(e => e.Description));
            return (false, error, null);
        }

        return (true, null, ToUserInfo(user));
    }

    public async Task<(bool Success, string? Error, UserInfo? User)> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return (false, "Invalid credentials.", null);

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);

        if (!result.Succeeded)
            return (false, "Invalid credentials.", null);

        return (true, null, ToUserInfo(user));
    }

    public async Task<(bool Success, string? Error, UserInfo? User)> LoginOrCreateOAuthUserAsync(
        string email, string name, string? avatarUrl)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                DisplayName = name,
                AvatarUrl = avatarUrl,
                IsProfileComplete = !string.IsNullOrEmpty(name),
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                var error = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, error, null);
            }
        }
        else if (avatarUrl != null && user.AvatarUrl != avatarUrl)
        {
            user.AvatarUrl = avatarUrl;
            await _userManager.UpdateAsync(user);
        }

        return (true, null, ToUserInfo(user));
    }

    public async Task<string> CreateRefreshTokenAsync(Guid userId)
    {
        var tokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var refreshToken = new RefreshToken
        {
            Token = tokenValue,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(30)
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return tokenValue;
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && !rt.IsRevoked);

        if (token != null)
        {
            token.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<(bool Found, string? ResetToken)> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
            return (false, null);

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        return (true, resetToken);
    }

    public async Task<(bool Success, string? Error)> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return (false, "Kullanıcı bulunamadı.");

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded)
            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

        return (true, null);
    }

    public async Task<(bool Success, string? Error)> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return (false, "Kullanıcı bulunamadı.");

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (!result.Succeeded)
            return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

        return (true, null);
    }

    private static UserInfo ToUserInfo(ApplicationUser user) =>
        new(user.Id, user.Email!, user.DisplayName, user.AvatarUrl, user.IsProfileComplete);
}