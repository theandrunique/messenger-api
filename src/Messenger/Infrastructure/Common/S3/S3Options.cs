using Microsoft.Extensions.Options;

namespace Messenger.Infrastructure.Common.Files;

public class S3Options : IValidateOptions<S3Options>
{
    public string AccessKey { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public string ServiceUrl { get; init; } = null!;

    public ValidateOptionsResult Validate(string? name, S3Options options)
    {
        var failures = new List<string>();

        failures.Add("Some error here");

        return ValidateOptionsResult.Success;
    }
}
