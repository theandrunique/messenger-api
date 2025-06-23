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
    private readonly EventReceiversProvider _receiversProvider;
    private const string _streamName = "gateway-events";

    public MessengerGatewayService(
        IConnectionMultiplexer redis,
        IEventSerializer serializer,
        ILogger<MessengerGatewayService> logger,
        EventReceiversProvider receiversProvider)
    {
        _redis = redis;
        _serializer = serializer;
        _logger = logger;
        _receiversProvider = receiversProvider;
    }

    public async Task PublishAsync<TPayload>(GatewayEvent<TPayload> @event) where TPayload : IGatewayEventPayload
    {
        try
        {
            var db = _redis.GetDatabase();

            var receivers = await _receiversProvider.GetReceivers(@event.ChannelId);

            var eventData = new NameValueEntry[]
            {
                new NameValueEntry("eventType", @event.EventType.ToString()),
                new NameValueEntry("payload", _serializer.Serialize(@event.Payload)),
                new NameValueEntry("recipients", _serializer.Serialize(receivers.Select(r => r.ToString()))),
                new NameValueEntry("source", GatewayEvent<TPayload>.Source),
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
