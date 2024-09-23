using System.Text.Json;
using System.Text.Json.Serialization;

namespace MessengerAPI.Application.Common;

public static class JsonOptions
{
    /// <summary>
    /// Default json serializer options
    /// </summary>
    public static readonly JsonSerializerOptions Default = new JsonSerializerOptions
    {
        Converters =
        {
            new JsonStringEnumConverter()
        },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
