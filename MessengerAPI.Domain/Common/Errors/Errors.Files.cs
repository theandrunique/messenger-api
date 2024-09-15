using ErrorOr;

namespace MessengerAPI.Domain.Common.Errors;

public static class FilesErrors
{
    public static Error TooBig(long maxSizeInMB) => Error.Validation(
        "files.too-big",
        $"Maximum allowed upload size is {maxSizeInMB} MB");

}
