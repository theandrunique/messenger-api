namespace MessengerAPI.Gateway.Serializers;

internal interface IEventSerializer
{
    string Serialize<T>(T obj);
    T? Deserialize<T>(string json);
}
