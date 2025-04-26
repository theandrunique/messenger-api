using MessengerAPI.Gateway.Common;
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

    public async Task PublishAsync<TPayload>(GatewayEvent<TPayload> @event) where TPayload : IGatewayEventPayload
    {
        try
        {
            var db = _redis.GetDatabase();

            var eventData = new NameValueEntry[]
            {
                new NameValueEntry("eventType", @event.EventType.ToString()),
                new NameValueEntry("payload", _serializer.Serialize(@event.Payload)),
                new NameValueEntry("recipients", _serializer.Serialize(@event.Recipients)),
                new NameValueEntry("source", GatewayEvent<TPayload>.Source),
            };

            var result = await db.StreamAddAsync(_streamName, eventData);
            _logger.LogInformation(
                "Published event {StreamMessageId} ({GatewayEventType}) to {Stream}",
                result,
                @event.EventType,
                _streamName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {GatewayEventType}", @event.EventType);
            throw;
        }
    }
}
