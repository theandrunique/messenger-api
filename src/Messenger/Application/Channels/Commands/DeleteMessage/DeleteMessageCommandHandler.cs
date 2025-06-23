using MediatR;
using Messenger.Errors;
using Messenger.Application.Common.Interfaces;
using Messenger.Domain.Events;
using Messenger.Domain.Channels.Permissions;
using Messenger.Domain.Channels.ValueObjects;
using Messenger.Domain.Data.Messages;
using Messenger.Domain.Data.Channels;

namespace Messenger.Application.Channels.Commands.DeleteMessage;

public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, ErrorOr<Unit>>
{
    private readonly IClientInfoProvider _clientInfo;
    private readonly IMessageRepository _messageRepository;
    private readonly IChannelLoaderFactory _channelLoaderFactory;
    private readonly IChannelRepository _channelRepository;
    private readonly IMediator _mediator;

    public DeleteMessageCommandHandler(
        IClientInfoProvider clientInfo,
        IMessageRepository messageRepository,
        IMediator mediator,
        IChannelLoaderFactory channelLoaderFactory,
        IChannelRepository channelRepository)
    {
        _clientInfo = clientInfo;
        _messageRepository = messageRepository;
        _mediator = mediator;
        _channelLoaderFactory = channelLoaderFactory;
        _channelRepository = channelRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelLoaderFactory
            .CreateLoader()
            .WithId(request.ChannelId)
            .WithMember(_clientInfo.UserId)
            .LoadAsync();

        if (channel is null)
        {
            return Error.Channel.NotFound(request.ChannelId);
        }

        if (!channel.HasMember(_clientInfo.UserId))
        {
            return Error.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        var message = await _messageRepository.GetMessageByIdOrNullAsync(request.ChannelId, request.MessageId);
        if (message is null)
        {
            return Error.Channel.MessageNotFound(request.MessageId);
        }

        if (message.AuthorId != _clientInfo.UserId
            && !channel.MemberHasPermission(_clientInfo.UserId, ChannelPermission.MANAGE_MESSAGES))
        {
            return Error.Channel.InsufficientPermissions(channel.Id, ChannelPermission.MANAGE_MESSAGES);
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
