using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
using MessengerAPI.Data.Channels;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.Events;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddOrEditMessage;

public class AddOrEditMessageCommandHandler : IRequestHandler<AddOrEditMessageCommand, ErrorOr<MessageSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly AttachmentService _attachmentService;
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IIdGenerator _idGenerator;
    private readonly IPublisher _publisher;
    private readonly IClientInfoProvider _clientInfo;

    public AddOrEditMessageCommandHandler(
        IChannelRepository channelRepository,
        IMessageRepository messageRepository,
        AttachmentService attachmentService,
        IUserRepository userRepository,
        IIdGenerator idGenerator,
        IPublisher publisher,
        IClientInfoProvider clientInfo)
    {
        _channelRepository = channelRepository;
        _messageRepository = messageRepository;
        _attachmentService = attachmentService;
        _userRepository = userRepository;
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

        var initiator = await _userRepository.GetByIdOrNullAsync(_clientInfo.UserId);
        if (initiator is null)
        {
            throw new ArgumentException("User was expected to be found.");
        }

        List<Attachment>? attachments = null;

        if (request.Attachments?.Count > 0)
        {
            attachments = new();

            foreach (var fileData in request.Attachments)
            {
                var attachment = await _attachmentService.ValidateAndCreateAttachmentAsync(
                    fileData.UploadedFilename,
                    fileData.Filename,
                    cancellationToken);

                if (attachment.IsError)
                {
                    return attachment.Error;
                }

                attachments.Add(attachment.Value);
            }
        }

        Message? message;

        if (request.MessageId.HasValue)
        {
            message = await _messageRepository.GetMessageByIdOrNullAsync(request.ChannelId, request.MessageId.Value);
            if (message is null)
            {
                return ApiErrors.Channel.MessageNotFound(request.MessageId.Value);
            }

            if (message.AuthorId != initiator.Id)
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
                _idGenerator.CreateId(),
                request.ChannelId,
                initiator,
                request.Content,
                attachments);
            await _messageRepository.UpsertAsync(message);
            await _publisher.Publish(new MessageCreateDomainEvent(channel, message, initiator));
        }

        return MessageSchema.From(message);
    }
}
