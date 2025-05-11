using System.ComponentModel.DataAnnotations;

namespace Messenger.Options;

public class SmtpOptions
{
    [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535.")]
    public int Port { get; init; }
    [Required]
    public string Server { get; init; } = null!;
    [Required]
    public string Login { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
    [Required]
    public string FromEmail { get; set; } = null!;
    [Required]
    public string FromName { get; set; } = "Messenger";
    public bool EnableSsl { get; set; } = true;
}
