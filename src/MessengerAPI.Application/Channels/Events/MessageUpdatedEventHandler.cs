using MassTransit;
using MediatR;
using MessengerAPI.Domain.ChannelAggregate.Events;

namespace MessengerAPI.Application.Channels.Events;

public class MessageUpdatedEventHandler : INotificationHandler<MessageUpdated>
{
    private readonly IPublishEndpoint _publisher;

    public MessageUpdatedEventHandler(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }
    /// <summary>
    /// Send a notification about new message to all channel members
    /// </summary>
    /// <param name="notification"><see cref="MessageUpdated"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    public async Task Handle(MessageUpdated notification, CancellationToken cancellationToken)
    {
        await _publisher.Publish(notification.NewMessage, cancellationToken);
    }
}
