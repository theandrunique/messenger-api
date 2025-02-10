namespace MessengerAPI.Infrastructure.Common.Files;

public class S3Options
{
    public string AccessKey { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public string BucketName { get; init; } = null!;
    public string EndpointUrl { get; init; } = null!;
    public string BucketUrl { get; init; } = null!;
    public int UploadPartSize { get; init; }
}
