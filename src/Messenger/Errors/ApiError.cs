namespace Messenger.Errors;

public class ApiError
{
    public ErrorCode Code { get; }
    public string Message { get; }
    public Dictionary<string, List<string>>? Errors { get; }

    public Dictionary<string, object>? Metadata { get; }

    public ApiError(ErrorCode code, string message, Dictionary<string, object>? metadata = null)
        : this(code, message, null, metadata) { }

    public ApiError(
        ErrorCode code,
        string message,
        Dictionary<string, List<string>>? errors,
        Dictionary<string, object>? metadata = null)
    {
        Code = code;
        Message = message;
        Errors = errors;
        Metadata = metadata;
    }
}
