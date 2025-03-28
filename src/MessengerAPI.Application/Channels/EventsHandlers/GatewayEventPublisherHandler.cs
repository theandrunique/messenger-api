using MediatR;
using MessengerAPI.Application.Channels.Events;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.Events;
using MessengerAPI.Gateway;

namespace MessengerAPI.Application.Channels.EventsHandlers;

public class GatewayEventPublisherHandler
    : INotificationHandler<MessageCreateDomainEvent>,
      INotificationHandler<MessageUpdateDomainEvent>,
      INotificationHandler<ChannelCreateDomainEvent>,
      INotificationHandler<ChannelTitleUpdateDomainEvent>,
      INotificationHandler<ChannelMemberAddDomainEvent>,
      INotificationHandler<ChannelMemberRemoveDomainEvent>,
      INotificationHandler<MessageAckDomainEvent>

{
    private readonly IGatewayService _gateway;

    public GatewayEventPublisherHandler(IGatewayService gateway)
    {
        _gateway = gateway;
    }

    public Task Handle(MessageCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        var messageSchema = MessageSchema.From(@event.Message);

        return _gateway.PublishAsync(new MessageCreateGatewayEvent(
            messageSchema,
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString()),
            @event.Channel.Type));
    }

    public Task Handle(MessageUpdateDomainEvent @event, CancellationToken cancellationToken)
    {
        var messageSchema = MessageSchema.From(@event.Message);

        return _gateway.PublishAsync(new MessageUpdateGatewayEvent(
            messageSchema,
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString()),
            @event.Channel.Type));
    }

    public Task Handle(ChannelMemberAddDomainEvent @event, CancellationToken cancellationToken)
    {
        return _gateway.PublishAsync(new ChannelMemberAddGatewayEvent(
            UserPublicSchema.From(@event.MemberInfo),
            @event.Channel.Id,
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString())));
    }

    public Task Handle(ChannelTitleUpdateDomainEvent @event, CancellationToken cancellationToken)
    {
        return _gateway.PublishAsync(new ChannelUpdateGatewayEvent(
            ChannelSchema.From(@event.Channel)));
    }

    public Task Handle(ChannelMemberRemoveDomainEvent @event, CancellationToken cancellationToken)
    {
        return _gateway.PublishAsync(new ChannelMemberRemoveGatewayEvent(
            UserPublicSchema.From(@event.MemberInfo),
            @event.Channel.Id,
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString())));
    }

    public Task Handle(ChannelCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        var channelSchema = ChannelSchema.From(@event.Channel);

        return _gateway.PublishAsync(new ChannelCreateGatewayEvent(channelSchema));
    }

    public Task Handle(MessageAckDomainEvent @event, CancellationToken cancellationToken)
    {
        return _gateway.PublishAsync(new MessageAckGatewayEvent(
            @event.Channel.Id,
            @event.messageId,
            @event.initiatorId,
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString())));
    }
}
