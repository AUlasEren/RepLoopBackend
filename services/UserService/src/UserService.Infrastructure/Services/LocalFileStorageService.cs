using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using UserService.Application.Common.Interfaces;

namespace UserService.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<LocalFileStorageService> _logger;
    private const string AvatarsFolder = "avatars";

    public LocalFileStorageService(IWebHostEnvironment env, ILogger<LocalFileStorageService> logger)
    {
        _env = env;
        _logger = logger;
    }

    public async Task<string> SaveAvatarAsync(Guid userId, Stream fileStream, string contentType, CancellationToken cancellationToken = default)
    {
        var ext = contentType switch
        {
            "image/png"  => ".png",
            "image/jpeg" => ".jpg",
            "image/webp" => ".webp",
            _            => ".jpg"
        };

        var wwwroot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var avatarDir = Path.Combine(wwwroot, AvatarsFolder);
        Directory.CreateDirectory(avatarDir);

        var fileName = $"{userId}{ext}";
        var filePath = Path.Combine(avatarDir, fileName);

        await using var file = File.Create(filePath);
        await fileStream.CopyToAsync(file, cancellationToken);

        return $"/{AvatarsFolder}/{fileName}";
    }

    public void DeleteAvatar(string avatarUrl)
    {
        try
        {
            var wwwroot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var filePath = Path.Combine(wwwroot, avatarUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (File.Exists(filePath))
                File.Delete(filePath);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Avatar silinirken hata oluştu: {AvatarUrl}", avatarUrl);
        }
    }
}
