using Messenger.Domain.Messages.Metadata;
using Newtonsoft.Json;

namespace Messenger.Data.Scylla.Common;

public class MessageMetadataConverter
{
    private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public static IMessageMetadata? ToValue(string? json)
    {
        if (json == null) return null;
        return JsonConvert.DeserializeObject<IMessageMetadata?>(json, _settings);
    }

    public static string? ToJson(IMessageMetadata? metadata)
    {
        return JsonConvert.SerializeObject(metadata, _settings);
    }
}
