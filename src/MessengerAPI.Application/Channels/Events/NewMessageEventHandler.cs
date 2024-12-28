using MassTransit;
using MediatR;
using MessengerAPI.Domain.Models.Events;

namespace MessengerAPI.Application.Channels.Events;

public class NewMessageEventHandler : INotificationHandler<NewMessageCreated>
{
    private readonly IPublishEndpoint _publisher;

    public NewMessageEventHandler(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }

    /// <summary>
    /// Send a notification about new message to all channel members
    /// </summary>
    /// <param name="notification"><see cref="NewMessageCreated"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    public async Task Handle(NewMessageCreated notification, CancellationToken cancellationToken)
    {
        await _publisher.Publish(notification.NewMessage, cancellationToken);
    }
}
