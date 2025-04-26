using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Domain.Events;
using Messenger.ApiErrors;
using Messenger.Domain.Entities;
using Messenger.Data.Interfaces.Channels;

namespace Messenger.Application.Channels.Commands.AckMessage;

public class AckMessageCommandHandler : IRequestHandler<AckMessageCommand, ErrorOr<Unit>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IPublisher _publisher;
    private readonly IMessageAckRepository _messageAckRepository;

    public AckMessageCommandHandler(
        IChannelRepository channelRepository,
        IClientInfoProvider clientInfo,
        IPublisher publisher,
        IMessageAckRepository messageAckRepository)
    {
        _channelRepository = channelRepository;
        _clientInfo = clientInfo;
        _publisher = publisher;
        _messageAckRepository = messageAckRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(AckMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel == null)
        {
            return Errors.Channel.NotFound(request.ChannelId);
        }
        var memberInfo = channel.FindMember(_clientInfo.UserId);
        if (memberInfo == null)
        {
            return Errors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        if (memberInfo.LastReadMessageId >= request.MessageId)
        {
            return Unit.Value;
        }

        var newAck = new MessageAck(channel.Id, memberInfo.UserId, request.MessageId);

        await _messageAckRepository.UpsertMessageAckStatus(newAck);

        await _publisher.Publish(new MessageAckDomainEvent(newAck.LastReadMessageId, channel, _clientInfo.UserId));

        return Unit.Value;
    }
}
