using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace Messenger.Options;

public class ApplicationOptions
{
    [ValidateObjectMembers]
    [Required]
    public S3Options S3Options { get; init; } = null!;

    [ValidateObjectMembers]
    [Required]
    public RedisOptions RedisOptions { get; init; } = null!;

    [ValidateObjectMembers]
    [Required]
    public ElasticSearchOptions ElasticSearchOptions { get; init; } = null!;

    [ValidateObjectMembers]
    [Required]
    public SmtpOptions SmtpOptions { get; init; } = null!;

    [ValidateObjectMembers]
    [Required]
    public ScyllaDBOptions ScyllaDBOptions { get; init; } = null!;

    [ValidateObjectMembers]
    [Required]
    public AuthOptions AuthOptions { get; init; } = null!;

    [ValidateObjectMembers]
    [Required]
    public AvatarServiceOptions AvatarServiceOptions { get; init; } = null!;

    [ValidateObjectMembers]
    [Required]
    public ChannelImageServiceOptions ChannelImageServiceOptions { get; init; } = null!;

    [ValidateObjectMembers]
    [Required]
    public AttachmentServiceOptions AttachmentServiceOptions { get; init; } = null!;

    [ValidateObjectMembers]
    [Required]
    public HCaptchaOptions HCaptchaOptions { get; init; } = null!;
}
