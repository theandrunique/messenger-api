using System.ComponentModel.DataAnnotations;

namespace Messenger.Options;

public class S3Options
{
    [Required(ErrorMessage = "AccessKey is required.")]
    public string AccessKey { get; init; } = null!;

    [Required(ErrorMessage = "SecretKey is required.")]
    public string SecretKey { get; init; } = null!;

    [Required(ErrorMessage = "ServiceUrl is required.")]
    [Url(ErrorMessage = "ServiceUrl must be a valid URL.")]
    public string ServiceUrl { get; init; } = null!;

    [Required]
    public bool ForcePathStyle { get; init; }
}
