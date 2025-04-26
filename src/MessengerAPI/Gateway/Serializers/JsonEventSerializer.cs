using System.Text.Json;
using System.Text.Json.Serialization;

namespace MessengerAPI.Gateway.Serializers;

internal class JsonEventSerializer : IEventSerializer
{
    private readonly JsonSerializerOptions _settings = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
    };

    public T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _settings);
    }

    public string Serialize<T>(T obj)
    {
        return JsonSerializer.Serialize<T>(obj, _settings);
    }
}
