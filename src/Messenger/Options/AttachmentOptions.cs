using System.ComponentModel.DataAnnotations;

namespace Messenger.Options;

public record AttachmentServiceOptions
{
    [Required]
    public string BucketName { get; init; } = null!;

    public TimeSpan UploadUrlExpiration { get; init; } = TimeSpan.FromDays(7);

    public TimeSpan DownloadUrlExpiration { get; init; } = TimeSpan.FromDays(7);
}
