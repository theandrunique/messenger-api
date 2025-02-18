namespace MessengerAPI.Infrastructure.Common.Files;

public class S3Options
{
    public string AccessKey { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public string ServiceUrl { get; init; } = null!;
}
