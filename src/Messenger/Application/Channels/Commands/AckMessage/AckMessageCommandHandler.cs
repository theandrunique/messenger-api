using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Domain.Events;
using Messenger.Errors;
using Messenger.Domain.Channels;
using Messenger.Domain.Data.Messages;
using Messenger.Domain.Data.Channels;

namespace Messenger.Application.Channels.Commands.AckMessage;

public class AckMessageCommandHandler : IRequestHandler<AckMessageCommand, ErrorOr<Unit>>
{
    private readonly IClientInfoProvider _clientInfo;
    private readonly IPublisher _publisher;
    private readonly IMessageAckRepository _messageAckRepository;
    private readonly IChannelLoaderFactory _channelLoaderFactory;

    public AckMessageCommandHandler(
        IClientInfoProvider clientInfo,
        IPublisher publisher,
        IMessageAckRepository messageAckRepository,
        IChannelLoaderFactory channelLoaderFactory)
    {
        _clientInfo = clientInfo;
        _publisher = publisher;
        _messageAckRepository = messageAckRepository;
        _channelLoaderFactory = channelLoaderFactory;
    }

    public async Task<ErrorOr<Unit>> Handle(AckMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelLoaderFactory
            .CreateLoader()
            .WithId(request.ChannelId)
            .WithMember(_clientInfo.UserId)
            .LoadAsync();

        if (channel == null)
        {
            return Error.Channel.NotFound(request.ChannelId);
        }

        var memberInfo = channel.ActiveMembers.FirstOrDefault(m => m.UserId == _clientInfo.UserId);
        if (memberInfo == null)
        {
            return Error.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
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
