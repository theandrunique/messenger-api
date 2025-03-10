namespace MessengerAPI.Application.Common.Interfaces.S3;

public interface IS3Service
{
    Task PutObjectAsync(
        string key,
        string bucket,
        string contentType,
        Stream fileStream,
        string? fileName = null,
        CancellationToken cancellationToken = default);

    string GenerateUploadUrl(string key, string bucket, DateTimeOffset expires, long size);

    string GenerateDownloadUrl(string key, string bucket, DateTimeOffset expires);

    Task<GetObjectMetadataResponseDTO?> GetObjectMetadataAsync(
        string key,
        string bucket,
        CancellationToken cancellationToken = default);

    Task DeleteObjectAsync(
        string key,
        string bucket,
        CancellationToken cancellationToken = default);

    Task<bool> IsObjectExistsAsync(
        string key,
        string bucket,
        CancellationToken cancellationToken = default);
}
