using MediatR;
using Messenger.ApiErrors;
using Messenger.Application.Common.Interfaces;
using Messenger.Data.Interfaces.Channels;
using Messenger.Domain.Channels;
using Messenger.Domain.Events;
using Messenger.Domain.ValueObjects;

namespace Messenger.Application.Channels.Commands.DeleteMessage;

public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, ErrorOr<Unit>>
{
    private readonly IClientInfoProvider _clientInfo;
    private readonly IChannelRepository _channelRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IMediator _mediator;

    public DeleteMessageCommandHandler(
        IClientInfoProvider clientInfo,
        IChannelRepository channelRepository,
        IMessageRepository messageRepository,
        IMediator mediator)
    {
        _clientInfo = clientInfo;
        _channelRepository = channelRepository;
        _messageRepository = messageRepository;
        _mediator = mediator;
    }

    public async Task<ErrorOr<Unit>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel is null)
        {
            return Errors.Channel.NotFound(request.ChannelId);
        }

        if (!channel.HasMember(_clientInfo.UserId))
        {
            return Errors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        var message = await _messageRepository.GetMessageByIdOrNullAsync(request.ChannelId, request.MessageId);
        if (message is null)
        {
            return Errors.Channel.MessageNotFound(request.MessageId);
        }

        if (message.AuthorId != _clientInfo.UserId && !channel.HasPermission(_clientInfo.UserId, ChannelPermission.MANAGE_MESSAGES))
        {
            return Errors.Channel.InsufficientPermissions(channel.Id, ChannelPermission.MANAGE_MESSAGES);
        }
        await _messageRepository.DeleteMessageByIdAsync(channel.Id, message.Id);

        var lastMessage = (await _messageRepository.GetMessagesAsync(
            channelId: channel.Id,
            before: long.MaxValue,
            limit: 1)).FirstOrDefault();

        MessageInfo? lastMessageInfo = lastMessage != null ? new MessageInfo(lastMessage) : null;

        await _channelRepository.UpdateLastMessage(channel.Id, lastMessageInfo);

        await _mediator.Publish(new MessageDeleteDomainEvent(
            channel,
            message.Id,
            lastMessage,
            _clientInfo.UserId));

        return Unit.Value;
    }
}
