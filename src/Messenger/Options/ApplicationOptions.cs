using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace Messenger.Options;

public class ApplicationOptions
{
    [ValidateObjectMembers]
    public S3Options S3Options { get; init; } = null!;

}
