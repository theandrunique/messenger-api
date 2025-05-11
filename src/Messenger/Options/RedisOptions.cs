using System.ComponentModel.DataAnnotations;

namespace Messenger.Options;

public class RedisOptions
{
    [Required(ErrorMessage = "ConnectionString is required.")]
    public string ConnectionString { get; init; } = null!;
}
