using MediatR;
using Messenger.Contracts.Common;
using Messenger.Domain.Events;
using Messenger.Gateway.Common;
using Messenger.Gateway.Events;

namespace Messenger.Gateway.EventPublishers;

public class ChannelGatewayEventsPublisher
    : INotificationHandler<ChannelCreateDomainEvent>,
      INotificationHandler<ChannelUpdateDomainEvent>,
      INotificationHandler<ChannelMemberAddDomainEvent>,
      INotificationHandler<ChannelMemberRemoveDomainEvent>
{
    private readonly IGatewayService _gateway;

    public ChannelGatewayEventsPublisher(IGatewayService gateway)
    {
        _gateway = gateway;
    }

    public async Task Handle(ChannelMemberAddDomainEvent @event, CancellationToken cancellationToken)
    {
        var member = UserPublicSchema.From(@event.MemberInfo);

        await _gateway.PublishAsync(new GatewayEvent<ChannelMemberAddGatewayEvent>(
            new ChannelMemberAddGatewayEvent(member, @event.Channel.Id),
            @event.Channel.Id));
    }

    public async Task Handle(ChannelUpdateDomainEvent @event, CancellationToken cancellationToken)
    {
        await _gateway.PublishAsync(new GatewayEvent<ChannelUpdateGatewayEvent>(
            new ChannelUpdateGatewayEvent(ChannelSchema.From(@event.Channel)),
            @event.Channel.Id));
    }

    public async Task Handle(ChannelMemberRemoveDomainEvent @event, CancellationToken cancellationToken)
    {
        var member = UserPublicSchema.From(@event.MemberInfo);

        await _gateway.PublishAsync(new GatewayEvent<ChannelMemberRemoveGatewayEvent>(
            new ChannelMemberRemoveGatewayEvent(member, @event.Channel.Id),
            @event.Channel.Id));
    }

    public async Task Handle(ChannelCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        await _gateway.PublishAsync(new GatewayEvent<ChannelCreateGatewayEvent>(
            new ChannelCreateGatewayEvent(ChannelSchema.From(@event.Channel)),
            @event.Channel.Id));
    }
}
