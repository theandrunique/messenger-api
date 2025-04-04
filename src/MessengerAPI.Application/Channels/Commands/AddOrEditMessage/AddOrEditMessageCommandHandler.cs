using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
using MessengerAPI.Data.Interfaces.Channels;
using MessengerAPI.Domain.Channels;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.Events;
using MessengerAPI.Domain.ValueObjects;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddOrEditMessage;

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
            return ApiErrors.Channel.NotFound(request.ChannelId);
        }

        if (!channel.HasMember(_clientInfo.UserId))
        {
            return ApiErrors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }
        if (!channel.HasPermission(_clientInfo.UserId, ChannelPermissions.SEND_MESSAGES))
        {
            return ApiErrors.Channel.InsufficientPermissions(channel.Id, ChannelPermissions.SEND_MESSAGES);
        }

        var initiator = channel.ActiveMembers.First(m => m.UserId == _clientInfo.UserId);

        List<Attachment>? attachments = null;
        if (request.Attachments?.Count > 0)
        {
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
        if (request.MessageId.HasValue)
        {
            message = await _messageRepository.GetMessageByIdOrNullAsync(request.ChannelId, request.MessageId.Value);
            if (message is null)
            {
                return ApiErrors.Channel.MessageNotFound(request.MessageId.Value);
            }

            if (message.AuthorId != initiator.UserId)
            {
                return ApiErrors.Channel.MessageWasSentByAnotherUser(request.MessageId.Value);
            }

            message.Edit(request.Content, attachments);
            await _messageRepository.UpsertAsync(message);
            await _publisher.Publish(new MessageUpdateDomainEvent(channel, message, initiator));
        }
        else
        {
            message = new Message(
                type: MessageType.DEFAULT,
                id: _idGenerator.CreateId(),
                channelId: request.ChannelId,
                author: initiator,
                content: request.Content,
                attachments: attachments);

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
