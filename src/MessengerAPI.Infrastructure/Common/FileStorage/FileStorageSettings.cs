using MessengerAPI.Application.Common.Interfaces;

namespace MessengerAPI.Infrastructure.Common.FileStorage;

public class FileStorageSettings : IFileStorageSettings
{
    public string AccessKey { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public string BucketName { get; init; } = null!;
    public string EndpointUrl { get; init; } = null!;
    public string BucketUrl { get; init; } = null!;
    public long MaxFileSize => MaxFileSizeInMB * 1024 * 1024;
    public long MaxFileSizeInMB { get; init; }
}
