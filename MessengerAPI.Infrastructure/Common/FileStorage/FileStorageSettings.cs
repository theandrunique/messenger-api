namespace MessengerAPI.Infrastructure.Common.FileStorage;

public class FileStorageSettings
{
    public string AccessKey { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public string BucketName { get; init; } = null!;
    public string EndpointUrl { get; init; } = null!;
    public string BucketUrl { get; init; } = null!;
}
