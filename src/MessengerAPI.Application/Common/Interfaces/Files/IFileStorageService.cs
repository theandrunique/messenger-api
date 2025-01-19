namespace MessengerAPI.Application.Common.Interfaces.Files;

public interface IFileStorageService
{
    Task PutObjectAsync(Stream fileStream, string key, string fileName, string contentType, CancellationToken cancellationToken);
    Task<string> GeneratePreSignedUrlForUploadAsync(string key, DateTimeOffset expires, long size);
    Task<GetObjectMetadataResponseDTO> GetObjectMetadataAsync(string key, CancellationToken cancellationToken);
    Task<string> GeneratePreSignedUrlForDownloadAsync(string key, DateTimeOffset expires);
    Task DeleteObjectAsync(string key, CancellationToken cancellationToken);
    Task<bool> IsObjectExistsAsync(string key, CancellationToken cancellationToken);
}
