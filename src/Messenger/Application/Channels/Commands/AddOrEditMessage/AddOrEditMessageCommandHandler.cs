using MediatR;
using Messenger.Application.Channels.Common;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Core;
using Messenger.Domain.Channels.Permissions;
using Messenger.Domain.Channels.ValueObjects;
using Messenger.Domain.Data.Channels;
using Messenger.Domain.Data.Messages;
using Messenger.Domain.Events;
using Messenger.Domain.Messages;
using Messenger.Domain.Messages.ValueObjects;
using Messenger.Errors;

namespace Messenger.Application.Channels.Commands.AddOrEditMessage;

public class AddOrEditMessageCommandHandler : IRequestHandler<AddOrEditMessageCommand, ErrorOr<MessageSchema>>
{
    private readonly AttachmentService _attachmentService;
    private readonly IMessageRepository _messageRepository;
    private readonly IIdGenerator _idGenerator;
    private readonly IPublisher _publisher;
    private readonly IClientInfoProvider _clientInfo;
    private readonly IChannelLoaderFactory _channelLoaderFactory;

    public AddOrEditMessageCommandHandler(
        IMessageRepository messageRepository,
        AttachmentService attachmentService,
        IIdGenerator idGenerator,
        IPublisher publisher,
        IClientInfoProvider clientInfo,
        IChannelLoaderFactory channelLoaderFactory)
    {
        _messageRepository = messageRepository;
        _attachmentService = attachmentService;
        _idGenerator = idGenerator;
        _publisher = publisher;
        _clientInfo = clientInfo;
        _channelLoaderFactory = channelLoaderFactory;
    }

    public async Task<ErrorOr<MessageSchema>> Handle(AddOrEditMessageCommand request, CancellationToken cancellationToken)
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

        var initiator = channel.ActiveMembers.FirstOrDefault(m => m.UserId == _clientInfo.UserId);
        if (initiator == null)
        {
            return Error.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }
        if (!channel.MemberHasPermission(_clientInfo.UserId, ChannelPermission.SEND_MESSAGES))
        {
            return Error.Channel.InsufficientPermissions(channel.Id, ChannelPermission.SEND_MESSAGES);
        }

        List<Attachment>? attachments = null;
        if (request.Attachments?.Count > 0)
        {
            if (!channel.MemberHasPermission(_clientInfo.UserId, ChannelPermission.ATTACH_FILES))
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
