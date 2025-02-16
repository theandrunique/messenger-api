using MessengerAPI.Core;
using MessengerAPI.Gateway.Serializers;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace MessengerAPI.Gateway;

internal class MessengerGatewayService : IGatewayService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IEventSerializer _serializer;
    private readonly ILogger<MessengerGatewayService> _logger;
    private const string _streamName = "gateway-events";

    public MessengerGatewayService(
        IConnectionMultiplexer redis,
        IEventSerializer serializer,
        ILogger<MessengerGatewayService> logger)
    {
        _redis = redis;
        _serializer = serializer;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IGatewayEvent
    {
        try
        {
            var db = _redis.GetDatabase();

            var eventData = new NameValueEntry[]
            {
                new NameValueEntry("eventType", @event.EventType.ToString()),
                new NameValueEntry("payload", _serializer.Serialize(@event))
            };

            await db.StreamAddAsync(_streamName, eventData);
            _logger.LogInformation("Published event {EventType} to {Stream}", @event.EventType, _streamName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {eventType}", @event.EventType);
            throw;
        }
    }
}
