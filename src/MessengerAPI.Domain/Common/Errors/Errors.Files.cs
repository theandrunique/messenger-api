using ErrorOr;

namespace MessengerAPI.Domain.Common.Errors;

public static class FilesErrors
{
    /// <summary>
    /// Error when file is too big
    /// </summary>
    /// <param name="maxSizeInMB">Maximum size in MB</param>
    public static Error TooBig(long maxSizeInMB) => Error.Validation(
        "files.too-big",
        $"Maximum allowed upload size is {maxSizeInMB} MB");

}
