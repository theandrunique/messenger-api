using MessengerAPI.Application.Files.Common.Interfaces;

namespace MessengerAPI.Infrastructure.Common.Files;

public class StorageOptions : IStorageOptions
{
    public string AccessKey { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public string BucketName { get; init; } = null!;
    public string EndpointUrl { get; init; } = null!;
    public string BucketUrl { get; init; } = null!;
    public int UploadPartSize { get; init; }
    public int SomeExperementalValue { get; init; }
    public long MaxFileSize => MaxFileSizeInMB * 1024 * 1024;
    public long MaxFileSizeInMB { get; init; }
}
