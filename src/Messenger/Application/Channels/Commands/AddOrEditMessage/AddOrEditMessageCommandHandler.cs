using MediatR;
using Messenger.Application.Channels.Common;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Core;
using Messenger.Data.Interfaces.Channels;
using Messenger.Domain.Channels;
using Messenger.Domain.Entities;
using Messenger.Domain.Events;
using Messenger.Domain.ValueObjects;
using Messenger.Errors;

namespace Messenger.Application.Channels.Commands.AddOrEditMessage;

public class AddOrEditMessageCommandHandler : IRequestHandler<AddOrEditMessageCommand, ErrorOr<MessageSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly AttachmentService _attachmentService;
    private readonly IMessageRepository _messageRepository;
    private readonly IIdGenerator _idGenerator;
    private readonly IPublisher _publisher;
    private readonly IClientInfoProvider _clientInfo;

    public AddOrEditMessageCommandHandler(
        IChannelRepository channelRepository,
        IMessageRepository messageRepository,
        AttachmentService attachmentService,
        IIdGenerator idGenerator,
        IPublisher publisher,
        IClientInfoProvider clientInfo)
    {
        _channelRepository = channelRepository;
        _messageRepository = messageRepository;
        _attachmentService = attachmentService;
        _idGenerator = idGenerator;
        _publisher = publisher;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<MessageSchema>> Handle(AddOrEditMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel is null)
        {
            return Error.Channel.NotFound(request.ChannelId);
        }

        if (!channel.HasMember(_clientInfo.UserId))
        {
            return Error.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }
        if (!channel.HasPermission(_clientInfo.UserId, ChannelPermission.SEND_MESSAGES))
        {
            return Error.Channel.InsufficientPermissions(channel.Id, ChannelPermission.SEND_MESSAGES);
        }

        var initiator = channel.ActiveMembers.First(m => m.UserId == _clientInfo.UserId);

        List<Attachment>? attachments = null;
        if (request.Attachments?.Count > 0)
        {
            if (!channel.HasPermission(_clientInfo.UserId, ChannelPermission.ATTACH_FILES))
            {
                return Error.Channel.InsufficientPermissions(channel.Id, ChannelPermission.ATTACH_FILES);
            }

            var attachmentTasks = request.Attachments.Select(f =>
                _attachmentService.ValidateAndCreateAttachmentAsync(
                    f.UploadedFilename,
                    f.Filename,
                    cancellationToken))
                .ToList();

            var attachmentResults = await Task.WhenAll(attachmentTasks);

            foreach (var result in attachmentResults)
            {
                if (result.IsError)
                {
                    return result.Error;
                }
            }
            attachments = attachmentResults.Select(r => r.Value).ToList();
        }

        Message? message;
        Message? referencedMessage = null;

        if (request.ReferencedMessageId != null)
        {
            referencedMessage = await _messageRepository.GetMessageByIdOrNullAsync(
                request.ChannelId,
                request.ReferencedMessageId.Value);

            if (referencedMessage == null)
            {
                return Error.Channel.MessageNotFound(request.ReferencedMessageId.Value);
            }
        }

        if (request.MessageId.HasValue)
        {
            message = await _messageRepository.GetMessageByIdOrNullAsync(request.ChannelId, request.MessageId.Value);
            if (message is null)
            {
                return Error.Channel.MessageNotFound(request.MessageId.Value);
            }

            if (message.AuthorId != initiator.UserId)
            {
                return Error.Channel.MessageWasSentByAnotherUser(request.MessageId.Value);
            }

            message.Edit(request.Content, attachments);
            await _messageRepository.UpsertAsync(message);
            await _publisher.Publish(new MessageUpdateDomainEvent(channel, message, initiator));
        }
        else
        {
            message = new Message(
                type: request.ReferencedMessageId.HasValue ? MessageType.REPLY : MessageType.DEFAULT,
                id: _idGenerator.CreateId(),
                channelId: request.ChannelId,
                author: initiator,
                content: request.Content,
                attachments: attachments,
                referencedMessage: referencedMessage);

            await _messageRepository.UpsertAsync(message);

            if (channel.LastMessage == null && channel.Type == ChannelType.DM)
            {
                await _publisher.Publish(new ChannelCreateDomainEvent(channel));
            }

            await _publisher.Publish(new MessageCreateDomainEvent(channel, message, initiator));
        }

        return MessageSchema.From(message);
    }
}
