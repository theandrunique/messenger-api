using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.Events;
using MessengerAPI.Gateway.Common;
using MessengerAPI.Gateway.Events;

namespace MessengerAPI.Gateway.EventPublishers;

public class MessageGatewayEventsPublisher
    : INotificationHandler<MessageCreateDomainEvent>,
      INotificationHandler<MessageUpdateDomainEvent>,
      INotificationHandler<MessageAckDomainEvent>
{
    private readonly IGatewayService _gateway;

    public MessageGatewayEventsPublisher(IGatewayService gateway)
    {
        _gateway = gateway;
    }

    public Task Handle(MessageCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        var messageSchema = MessageSchema.From(@event.Message);

        return _gateway.PublishAsync(new GatewayEvent<MessageCreateGatewayEvent>(
            new MessageCreateGatewayEvent(messageSchema, @event.Channel.Type),
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString())));
    }

    public Task Handle(MessageUpdateDomainEvent @event, CancellationToken cancellationToken)
    {
        var messageSchema = MessageSchema.From(@event.Message);

        return _gateway.PublishAsync(new GatewayEvent<MessageUpdateGatewayEvent>(
            new MessageUpdateGatewayEvent(messageSchema, @event.Channel.Type),
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString())));
    }

    public Task Handle(MessageAckDomainEvent @event, CancellationToken cancellationToken)
    {
        return _gateway.PublishAsync(new GatewayEvent<MessageAckGatewayEvent>(
            new MessageAckGatewayEvent(@event.Channel.Id, @event.messageId, @event.initiatorId),
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString())));
    }
}
