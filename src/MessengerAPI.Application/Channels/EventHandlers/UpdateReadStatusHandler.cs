using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Data.Channels;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.Events;

namespace MessengerAPI.Application.Channels.EventHandlers;

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
        var memberInfo = @event.Channel.FindMember(_clientInfo.UserId);

        if (memberInfo == null)
        {
            throw new InvalidOperationException("Member was expected to be found here.");
        }

        if (memberInfo.ReadAt >= @event.Message.Id)
        {
            return;
        }

        var newAck = new MessageAck(@event.Channel.Id, memberInfo.UserId, @event.Message.Id);

        await _messageAckRepository.UpsertMessageAckStatus(newAck);

        await _publisher.Publish(new MessageAckDomainEvent(newAck.LastReadMessageId, @event.Channel, _clientInfo.UserId));
    }
}
