namespace Messenger.Application.Channels.Common;

public record AttachmentOptions
{
    public string Bucket { get; init; } = null!;
    public TimeSpan UploadUrlExpiration { get; init; } = TimeSpan.FromDays(7);
    public TimeSpan DownloadUrlExpiration { get; init; } = TimeSpan.FromDays(7);
}
