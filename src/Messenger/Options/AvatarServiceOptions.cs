using System.ComponentModel.DataAnnotations;

namespace Messenger.Options;

public class AvatarServiceOptions
{
    [Required]
    public string BucketName { get; init; } = null!;
}
