using System.ComponentModel.DataAnnotations;

namespace Messenger.Options;

public class AuthOptions
{
    [Required]
    [ValidKeyDirectory("*.pem")]
    public string KeysDirectory { get; init; } = null!;
    [Required]
    [Range(1, int.MaxValue)]
    public int AccessTokenExpiryMinutes { get; init; }
    [Required]
    public string Issuer { get; init; } = null!;
    [Required]
    public string Audience { get; init; } = null!;
    [Required]
    public string RevokedTokensCacheKeyPrefix { get; init; } = null!;
}

public class ValidKeyDirectoryAttribute : ValidationAttribute
{
    public string SearchPattern { get; }

    public ValidKeyDirectoryAttribute(string searchPattern = "*.pem")
    {
        SearchPattern = searchPattern;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string path || string.IsNullOrWhiteSpace(path))
            return new ValidationResult("KeysDirectory is required.");

        if (!Directory.Exists(path))
            return new ValidationResult($"Directory '{path}' does not exist.");

        var files = Directory.GetFiles(path, SearchPattern, SearchOption.TopDirectoryOnly);
        if (files.Length == 0)
            return new ValidationResult($"Directory '{path}' contains no '{SearchPattern}' files.");

        return ValidationResult.Success;
    }
}
