namespace Messenger.Gateway.Serializers;

public class ProtobufEventSerializer : IEventSerializer
{
    public T? Deserialize<T>(byte[] data)
    {
        using var stream = new MemoryStream(data);
        return ProtoBuf.Serializer.Deserialize<T>(stream);
    }

    public byte[] Serialize<T>(T obj)
    {
        using var ms = new MemoryStream();
        ProtoBuf.Serializer.Serialize(ms, obj);
        return ms.ToArray();
    }
}
