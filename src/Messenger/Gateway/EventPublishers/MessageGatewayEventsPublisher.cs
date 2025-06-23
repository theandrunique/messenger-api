using MediatR;
using Messenger.Contracts.Common;
using Messenger.Domain.Events;
using Messenger.Gateway.Common;
using Messenger.Gateway.Events;

namespace Messenger.Gateway.EventPublishers;

public class MessageGatewayEventsPublisher
    : INotificationHandler<MessageCreateDomainEvent>,
      INotificationHandler<MessageUpdateDomainEvent>,
      INotificationHandler<MessageAckDomainEvent>,
      INotificationHandler<MessageDeleteDomainEvent>
{
    private readonly IGatewayService _gateway;

    public MessageGatewayEventsPublisher(IGatewayService gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(MessageCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        var messageSchema = MessageSchema.From(@event.Message);

        await _gateway.PublishAsync(new GatewayEvent<MessageCreateGatewayEvent>(
            new MessageCreateGatewayEvent(messageSchema, @event.Channel.Type),
            @event.Channel.Id));
    }

    public async Task Handle(MessageUpdateDomainEvent @event, CancellationToken cancellationToken)
    {
        var messageSchema = MessageSchema.From(@event.Message);

        await _gateway.PublishAsync(new GatewayEvent<MessageUpdateGatewayEvent>(
            new MessageUpdateGatewayEvent(messageSchema, @event.Channel.Type),
            @event.Channel.Id));
    }

    public async Task Handle(MessageAckDomainEvent @event, CancellationToken cancellationToken)
    {
        await _gateway.PublishAsync(new GatewayEvent<MessageAckGatewayEvent>(
            new MessageAckGatewayEvent(@event.Channel.Id, @event.messageId, @event.initiatorId),
            @event.Channel.Id));
    }

    public async Task Handle(MessageDeleteDomainEvent @event, CancellationToken cancellationToken)
    {
        await _gateway.PublishAsync(new GatewayEvent<MessageDeleteGatewayEvent>(
            new MessageDeleteGatewayEvent(
                @event.Channel.Id,
                @event.MessageId,
                @event.NewLastMessage != null ? MessageSchema.From(@event.NewLastMessage) : null),
            @event.Channel.Id));
    }
}
