using MediatR;
using MessengerAPI.Application.Channels.Events;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
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

    public Task Handle(ChannelMemberAddDomainEvent @event, CancellationToken cancellationToken)
    {
        var member = UserPublicSchema.From(@event.MemberInfo);

        return _gateway.PublishAsync(new GatewayEvent<ChannelMemberAddGatewayEvent>(
            new ChannelMemberAddGatewayEvent(member, @event.Channel.Id),
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString())));
    }

    public Task Handle(ChannelTitleUpdateDomainEvent @event, CancellationToken cancellationToken)
    {
        return _gateway.PublishAsync(new GatewayEvent<ChannelUpdateGatewayEvent>(
            new ChannelUpdateGatewayEvent(ChannelSchema.From(@event.Channel)),
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString())));
    }

    public Task Handle(ChannelMemberRemoveDomainEvent @event, CancellationToken cancellationToken)
    {
        var member = UserPublicSchema.From(@event.MemberInfo);

        return _gateway.PublishAsync(new GatewayEvent<ChannelMemberRemoveGatewayEvent>(
            new ChannelMemberRemoveGatewayEvent(member, @event.Channel.Id),
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString())));
    }

    public Task Handle(ChannelCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        return _gateway.PublishAsync(new GatewayEvent<ChannelCreateGatewayEvent>(
            new ChannelCreateGatewayEvent(ChannelSchema.From(@event.Channel)),
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString())));
    }

    public Task Handle(MessageAckDomainEvent @event, CancellationToken cancellationToken)
    {
        return _gateway.PublishAsync(new GatewayEvent<MessageAckGatewayEvent>(
            new MessageAckGatewayEvent(@event.Channel.Id, @event.messageId, @event.initiatorId),
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString())));
    }
}
