using MassTransit;
using MediatR;
using MessengerAPI.Domain.Events;

namespace MessengerAPI.Application.Channels.Events;

public class MessageUpdateEventHandler : INotificationHandler<MessageUpdate>
{
    private readonly IPublishEndpoint _publisher;

    public MessageUpdateEventHandler(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }

    public async Task Handle(MessageUpdate notification, CancellationToken cancellationToken)
    {
        await _publisher.Publish(notification.Message, cancellationToken);
    }
}
