using System.Text.Json.Serialization;
using MessengerAPI.Errors;

namespace MessengerAPI.Contracts.Common;

public record ApiErrorSchema
{
    public int Code { get; init; }
    public string Message { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Dictionary<string, List<string>>? Errors { get; init; }

    public static ApiErrorSchema FromApiError(BaseApiError error) => new()
    {
        Code = (int)error.Code,
        Message = error.Message,
        Errors = error.Errors
    };
}

