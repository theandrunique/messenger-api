using MediatR;
using Messenger.Errors;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Core;
using Messenger.Data.Interfaces.Channels;
using Messenger.Domain.Events;
using Messenger.Domain.Channels.Permissions;

namespace Messenger.Application.Channels.Commands.ForwardMessages;

public class ForwardMessagesCommandHandler : IRequestHandler<ForwardMessagesCommand, ErrorOr<List<MessageSchema>>>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IChannelRepository _channelRepository;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IIdGenerator _idGenerator;
    private readonly IMediator _mediator;

    public ForwardMessagesCommandHandler(
        IMessageRepository messageRepository,
        IChannelRepository channelRepository,
        IClientInfoProvider clientInfo,
        IIdGenerator idGenerator,
        IMediator mediator)
    {
        _messageRepository = messageRepository;
        _channelRepository = channelRepository;
        _clientInfo = clientInfo;
        _idGenerator = idGenerator;
        _mediator = mediator;
    }

    public async Task<ErrorOr<List<MessageSchema>>> Handle(ForwardMessagesCommand request, CancellationToken cancellationToken)
    {
        var targetChannel = await _channelRepository.GetByIdOrNullAsync(request.TargetChannelId);
        if (targetChannel == null)
        {
            return Error.Channel.NotFound(request.TargetChannelId);
        }
        if (!targetChannel.HasMember(_clientInfo.UserId))
        {
            return Error.Channel.UserNotMember(_clientInfo.UserId, targetChannel.Id);
        }
        if (!targetChannel.HasPermission(_clientInfo.UserId, ChannelPermission.SEND_MESSAGES))
        {
            return Error.Channel.InsufficientPermissions(targetChannel.Id, ChannelPermission.SEND_MESSAGES);
        }

        var initiator = targetChannel.ActiveMembers.First(m => m.UserId == _clientInfo.UserId);

        var messages = (await _messageRepository.GetMessagesByIdsAsync(request.ChannelId, request.MessageIds))
            .ToList();

        foreach (var message in messages)
        {
            foreach (var attachment in message.Attachments)
            {
                attachment.Forward(
                    newChannelId: targetChannel.Id,
                    newId: _idGenerator.CreateId());
            }

            message.MakeForwarded(
                author: initiator,
                channelId: targetChannel.Id,
                messageId: _idGenerator.CreateId(),
                includeReferencedMessage: false,
                includeOriginAuthorLink: true);
        }

        await _messageRepository.BulkUpsertAsync(messages);
        foreach (var message in messages)
        {
            await _mediator.Publish(new MessageCreateDomainEvent(targetChannel, message, initiator));
        }

        return MessageSchema.From(messages);
    }
}
