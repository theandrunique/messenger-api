using Messenger.Gateway.Common;
using Messenger.Gateway.Serializers;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Messenger.Gateway;

internal class MessengerGatewayService : IGatewayService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IEventSerializer _serializer;
    private readonly ILogger<MessengerGatewayService> _logger;
    private const string _streamName = "gateway-events";
    public const string _source = "messenger-api";

    public MessengerGatewayService(
        IConnectionMultiplexer redis,
        IEventSerializer serializer,
        ILogger<MessengerGatewayService> logger)
    {
        _redis = redis;
        _serializer = serializer;
        _logger = logger;
    }

    public async Task PublishAsync(IGatewayEvent @event, IEnumerable<long> recipients)
    {
        try
        {
            var db = _redis.GetDatabase();

            var eventData = new NameValueEntry[]
            {
                new NameValueEntry("eventType", @event.EventType.ToString()),
                new NameValueEntry("payload", _serializer.Serialize((object)@event)),
                new NameValueEntry("recipients", _serializer.Serialize(recipients.Select(r => r.ToString()))),
                new NameValueEntry("source", _source),
            };

            var result = await db.StreamAddAsync(_streamName, eventData);

            _logger.LogDebug(
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
