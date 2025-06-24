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
    private readonly ChannelRecipientsProvider _recipientsProvider;

    public MessageGatewayEventsPublisher(IGatewayService gateway, ChannelRecipientsProvider recipientsProvider)
    {
        _gateway = gateway;
        _recipientsProvider = recipientsProvider;
    }

    public async Task Handle(MessageCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        var messageSchema = MessageSchema.From(@event.Message);

        await _gateway.PublishAsync(
            new MessageCreateGatewayEvent(messageSchema, @event.Channel.Type),
            await _recipientsProvider.GetRecipients(@event.Channel.Id));
    }

    public async Task Handle(MessageUpdateDomainEvent @event, CancellationToken cancellationToken)
    {
        var messageSchema = MessageSchema.From(@event.Message);

        await _gateway.PublishAsync(
            new MessageUpdateGatewayEvent(messageSchema, @event.Channel.Type),
            await _recipientsProvider.GetRecipients(@event.Channel.Id));
    }

    public async Task Handle(MessageAckDomainEvent @event, CancellationToken cancellationToken)
    {
        await _gateway.PublishAsync(
            new MessageAckGatewayEvent(@event.Channel.Id, @event.messageId, @event.initiatorId),
            await _recipientsProvider.GetRecipients(@event.Channel.Id));
    }

    public async Task Handle(MessageDeleteDomainEvent @event, CancellationToken cancellationToken)
    {
        await _gateway.PublishAsync(
            new MessageDeleteGatewayEvent(
                @event.Channel.Id,
                @event.MessageId,
                @event.NewLastMessage != null ? MessageSchema.From(@event.NewLastMessage) : null),
            await _recipientsProvider.GetRecipients(@event.Channel.Id));
    }
}
