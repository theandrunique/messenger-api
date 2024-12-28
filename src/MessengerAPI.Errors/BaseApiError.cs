namespace MessengerAPI.Errors;

public class BaseApiError
{
    public ErrorCode Code { get; }
    public string Message { get; }
    public Dictionary<string, List<string>>? Errors { get; }

    public BaseApiError(ErrorCode code, string message)
        : this(code, message, null) { }

    public BaseApiError(
        ErrorCode code,
        string message,
        Dictionary<string, List<string>>? errors)
    {
        Code = code;
        Message = message;
        Errors = errors;
    }
}

