using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Domain.Channels;
using Messenger.Domain.Data.Messages;
using Messenger.Domain.Events;

namespace Messenger.Application.Channels.EventHandlers;

public class UpdateReadStatusHandler : INotificationHandler<MessageCreateDomainEvent>
{
    private readonly IClientInfoProvider _clientInfo;
    private readonly IMessageAckRepository _messageAckRepository;
    private readonly IMediator _publisher;

    public UpdateReadStatusHandler(
        IClientInfoProvider clientInfo,
        IMessageAckRepository messageAckRepository,
        IMediator publisher)
    {
        _clientInfo = clientInfo;
        _messageAckRepository = messageAckRepository;
        _publisher = publisher;
    }

    public async Task Handle(MessageCreateDomainEvent @event, CancellationToken cancellationToken)
    {
        var memberInfo = @event.Channel.ActiveMembers.FirstOrDefault(m => m.UserId == _clientInfo.UserId);

        if (memberInfo == null)
        {
            throw new InvalidOperationException("Member was expected to be found here.");
        }

        if (memberInfo.LastReadMessageId >= @event.Message.Id)
        {
            return;
        }

        var newAck = new MessageAck(@event.Channel.Id, memberInfo.UserId, @event.Message.Id);

        await _messageAckRepository.UpsertMessageAckStatus(newAck);

        await _publisher.Publish(new MessageAckDomainEvent(newAck.LastReadMessageId, @event.Channel, _clientInfo.UserId));
    }
}
