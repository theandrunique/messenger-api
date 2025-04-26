using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.Events;
using MessengerAPI.Gateway.Common;
using MessengerAPI.Gateway.Events;

namespace MessengerAPI.Gateway.EventPublishers;

public class ChannelGatewayEventsPublisher
    : INotificationHandler<ChannelCreateDomainEvent>,
      INotificationHandler<ChannelNameUpdateDomainEvent>,
      INotificationHandler<ChannelMemberAddDomainEvent>,
      INotificationHandler<ChannelMemberRemoveDomainEvent>
{
    private readonly IGatewayService _gateway;

    public ChannelGatewayEventsPublisher(IGatewayService gateway)
    {
        _gateway = gateway;
    }

    public Task Handle(ChannelMemberAddDomainEvent @event, CancellationToken cancellationToken)
    {
        var member = UserPublicSchema.From(@event.MemberInfo);

        return _gateway.PublishAsync(new GatewayEvent<ChannelMemberAddGatewayEvent>(
            new ChannelMemberAddGatewayEvent(member, @event.Channel.Id),
            @event.Channel.ActiveMembers.Select(m => m.UserId.ToString())));
    }

    public Task Handle(ChannelNameUpdateDomainEvent @event, CancellationToken cancellationToken)
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
}
