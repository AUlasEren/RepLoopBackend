using Microsoft.EntityFrameworkCore;
using RepLoopBackend.Application;
using RepLoopBackend.Application.Common.Interfaces;
using RepLoopBackend.Persistence.Entities;

namespace RepLoopBackend.Persistence.Services;

public class PasswordResetCodeStore : IPasswordResetCodeStore
{
    private readonly ApplicationDbContext _db;

    private static readonly TimeSpan CooldownPeriod = TimeSpan.FromMinutes(2);
    private static readonly TimeSpan CodeExpiry = TimeSpan.FromMinutes(60);
    private const int MaxAttempts = 5;

    public PasswordResetCodeStore(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> HasRecentCodeAsync(string email, CancellationToken ct = default)
    {
        var threshold = DateTime.UtcNow.Subtract(CooldownPeriod);
        return await _db.PasswordResetCodes
            .AnyAsync(p => p.Email == email && p.CreatedAt > threshold && !p.IsUsed, ct);
    }

    public async Task<string> CreateCodeAsync(string email, string identityToken, CancellationToken ct = default)
    {
        // Eski kullanılmamış kodları invalidate et
        var oldCodes = await _db.PasswordResetCodes
            .Where(p => p.Email == email && !p.IsUsed)
            .ToListAsync(ct);

        foreach (var old in oldCodes)
            old.IsUsed = true;

        var code = Random.Shared.Next(100_000, 999_999).ToString();

        _db.PasswordResetCodes.Add(new PasswordResetCode
        {
            Email = email,
            Code = code,
            Token = identityToken,
            ExpiresAt = DateTime.UtcNow.Add(CodeExpiry),
        });

        await _db.SaveChangesAsync(ct);
        return code;
    }

    public async Task<CodeVerifyResult> VerifyCodeAsync(string email, string code, CancellationToken ct = default)
    {
        var resetCode = await _db.PasswordResetCodes
            .Where(p => p.Email == email && !p.IsUsed)
            .OrderByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync(ct);

        if (resetCode == null)
            return new CodeVerifyResult(false, Error: "Geçersiz veya süresi dolmuş kod.",
                ErrorCode: ErrorCodes.ResetCodeInvalid);

        if (resetCode.ExpiresAt < DateTime.UtcNow)
        {
            resetCode.IsUsed = true;
            await _db.SaveChangesAsync(ct);
            return new CodeVerifyResult(false, Error: "Kodun süresi dolmuş. Lütfen yeni bir kod isteyin.",
                ErrorCode: ErrorCodes.ResetCodeExpired);
        }

        if (resetCode.AttemptCount >= MaxAttempts)
        {
            resetCode.IsUsed = true;
            await _db.SaveChangesAsync(ct);
            return new CodeVerifyResult(false, Error: "Çok fazla başarısız deneme. Lütfen yeni bir kod isteyin.",
                ErrorCode: ErrorCodes.ResetCodeMaxAttempts);
        }

        if (resetCode.Code != code.Trim())
        {
            resetCode.AttemptCount++;
            if (resetCode.AttemptCount >= MaxAttempts)
                resetCode.IsUsed = true;
            await _db.SaveChangesAsync(ct);

            var remaining = MaxAttempts - resetCode.AttemptCount;
            return new CodeVerifyResult(false,
                Error: remaining > 0
                    ? $"Geçersiz kod. {remaining} deneme hakkınız kaldı."
                    : "Çok fazla başarısız deneme. Lütfen yeni bir kod isteyin.",
                ErrorCode: remaining > 0 ? ErrorCodes.ResetCodeInvalid : ErrorCodes.ResetCodeMaxAttempts);
        }

        // Başarılı — kodu kullanılmış işaretle
        resetCode.IsUsed = true;
        await _db.SaveChangesAsync(ct);

        return new CodeVerifyResult(true, IdentityToken: resetCode.Token);
    }
}
