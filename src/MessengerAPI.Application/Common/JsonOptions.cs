using System.Text.Json;
using System.Text.Json.Serialization;

namespace MessengerAPI.Application.Common;

public static class JsonOptions
{
    public static readonly JsonSerializerOptions Default = new JsonSerializerOptions
    {
        Converters =
        {
            new JsonStringEnumConverter()
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
