namespace UserService.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveAvatarAsync(Guid userId, Stream fileStream, string contentType, CancellationToken cancellationToken = default);
    void DeleteAvatar(string avatarUrl);
}