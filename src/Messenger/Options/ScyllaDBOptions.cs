using System.ComponentModel.DataAnnotations;

namespace Messenger.Options;

public class ScyllaDBOptions
{
    [Required]
    public string[] ContactPoints { get; init; } = null!;

    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535.")]
    public int Port { get; init; } = 9042;

    [Required]
    public string DefaultKeyspace { get; init; } = null!;

    // public string[] ContactPoints { get; init; }
    // public int Port { get; set; } = 9042;
    // public string Keyspace { get; init; }
    // public string Username { get; init; }
    // public string Password { get; init; }
}
