using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Messenger.Gateway.Serializers;

internal class JsonEventSerializer : IEventSerializer
{
    private readonly JsonSerializerOptions _settings = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
    };

    public T? Deserialize<T>(byte[] data)
    {
        return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(data), _settings);
    }

    public byte[] Serialize<T>(T obj)
    {
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(obj, _settings));
    }
}
