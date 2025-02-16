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
      INotificationHandler<ChannelMemberAddDomainEvent>,
      INotificationHandler<ChannelMemberRemoveDomainEvent>

{
    private readonly IGatewayService _gateway;

    public GatewayEventPublisherHandler(IGatewayService gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(MessageCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        var messageSchema = MessageSchema.From(@event.Message);

        await _gateway.PublishAsync(new MessageCreateGatewayEvent(
            messageSchema,
            @event.Channel.Members.Select(m => m.UserId.ToString()),
            @event.Channel.Type));
    }

    public async Task Handle(MessageUpdateDomainEvent @event, CancellationToken cancellationToken)
    {
        var messageSchema = MessageSchema.From(@event.Message);

        await _gateway.PublishAsync(new MessageUpdateGatewayEvent(
            messageSchema,
            @event.Channel.Members.Select(m => m.UserId.ToString()),
            @event.Channel.Type));
    }

    public async Task Handle(ChannelMemberAddDomainEvent @event, CancellationToken cancellationToken)
    {
        await _gateway.PublishAsync(new ChannelMemberAddGatewayEvent(
            ChannelMemberInfoSchema.From(@event.MemberInfo),
            @event.Channel.Id,
            @event.Channel.Members.Select(m => m.UserId.ToString())
        ));
    }

    public async Task Handle(ChannelMemberRemoveDomainEvent @event, CancellationToken cancellationToken)
    {
        await _gateway.PublishAsync(new ChannelMemberRemoveGatewayEvent(
            ChannelMemberInfoSchema.From(@event.MemberInfo),
            @event.Channel.Id,
            @event.Channel.Members.Select(m => m.UserId.ToString())
        ));
    }

    public Task Handle(ChannelCreateDomainEvent notification, CancellationToken cancellationToken)
    {
        var channelSchema = ChannelSchema.From(notification.Channel);

        return _gateway.PublishAsync(new ChannelCreateGatewayEvent(channelSchema));
    }
}
