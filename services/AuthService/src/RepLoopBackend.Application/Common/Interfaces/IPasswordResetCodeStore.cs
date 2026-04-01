namespace RepLoopBackend.Application.Common.Interfaces;

public interface IPasswordResetCodeStore
{
    /// <summary>Aynı email için 2dk cooldown kontrolü.</summary>
    Task<bool> HasRecentCodeAsync(string email, CancellationToken ct = default);

    /// <summary>Eski kullanılmamış kodları invalidate et, yeni kodu kaydet, 6 haneli kodu döndür.</summary>
    Task<string> CreateCodeAsync(string email, string identityToken, CancellationToken ct = default);

    /// <summary>
    /// Kodu doğrula. Başarılıysa Identity token'ı döndür.
    /// Başarısızda attempt sayısını artır, max aşılırsa invalidate et.
    /// </summary>
    Task<CodeVerifyResult> VerifyCodeAsync(string email, string code, CancellationToken ct = default);
}

public record CodeVerifyResult(bool Success, string? IdentityToken = null, string? Error = null, string? ErrorCode = null);
