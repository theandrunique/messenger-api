using MassTransit;
using MediatR;
using MessengerAPI.Domain.Models.Events;

namespace MessengerAPI.Application.Channels.Events;

public class MessageCreateEventHandler : INotificationHandler<MessageCreate>
{
    private readonly IPublishEndpoint _publisher;

    public MessageCreateEventHandler(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }

    public async Task Handle(MessageCreate notification, CancellationToken cancellationToken)
    {
        await _publisher.Publish(notification.Message, cancellationToken);
    }
}
