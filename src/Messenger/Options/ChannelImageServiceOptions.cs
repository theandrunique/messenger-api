using System.ComponentModel.DataAnnotations;

namespace Messenger.Options;

public class ChannelImageServiceOptions
{
    [Required]
    public string BucketName { get; init; } = null!;
}
