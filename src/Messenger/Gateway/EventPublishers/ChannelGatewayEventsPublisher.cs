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
    private readonly ChannelRecipientsProvider _recipientsProvider;

    public ChannelGatewayEventsPublisher(IGatewayService gateway, ChannelRecipientsProvider recipientsProvider)
    {
        _gateway = gateway;
        _recipientsProvider = recipientsProvider;
    }

    public async Task Handle(ChannelMemberAddDomainEvent @event, CancellationToken cancellationToken)
    {
        var member = UserPublicSchema.From(@event.MemberInfo);

        await _gateway.PublishAsync(
            new ChannelMemberAddGatewayEvent(member, @event.Channel.Id),
            await _recipientsProvider.GetRecipients(@event.Channel.Id));
    }

    public async Task Handle(ChannelUpdateDomainEvent @event, CancellationToken cancellationToken)
    {
        var e = new ChannelUpdateGatewayEvent(@event.Channel.Id);

        e.Image = @event.NewImage;
        e.Name = @event.NewName;

        await _gateway.PublishAsync(e, await _recipientsProvider.GetRecipients(@event.Channel.Id));
    }

    public async Task Handle(ChannelMemberRemoveDomainEvent @event, CancellationToken cancellationToken)
    {
        var member = UserPublicSchema.From(@event.MemberInfo);

        var recipients = await _recipientsProvider.GetRecipients(@event.Channel.Id);
        recipients.Add(@event.MemberInfo.UserId);

        await _gateway.PublishAsync(
            new ChannelMemberRemoveGatewayEvent(member, @event.Channel.Id),
            recipients);
    }

    public async Task Handle(ChannelCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        await _gateway.PublishAsync(
            new ChannelCreateGatewayEvent(ChannelSchema.From(@event.Channel)),
            await _recipientsProvider.GetRecipients(@event.Channel.Id));
    }
}
