namespace Messenger.Gateway.Serializers;

internal interface IEventSerializer
{
    byte[] Serialize<T>(T obj);
    T? Deserialize<T>(byte[] data);
}
