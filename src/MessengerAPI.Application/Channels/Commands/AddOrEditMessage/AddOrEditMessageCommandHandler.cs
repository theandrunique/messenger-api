using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Application.Channels.Events;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
using MessengerAPI.Data.Channels;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Errors;
using MessengerAPI.Gateway;

namespace MessengerAPI.Application.Channels.Commands.AddOrEditMessage;

public class AddOrEditMessageCommandHandler : IRequestHandler<AddOrEditMessageCommand, ErrorOr<MessageSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly AttachmentService _attachmentService;
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IIdGenerator _idGenerator;
    private readonly IGatewayService _gateway;
    private readonly IClientInfoProvider _clientInfo;

    public AddOrEditMessageCommandHandler(
        IChannelRepository channelRepository,
        IMessageRepository messageRepository,
        AttachmentService attachmentService,
        IUserRepository userRepository,
        IIdGenerator idGenerator,
        IGatewayService gateway,
        IClientInfoProvider clientInfo)
    {
        _channelRepository = channelRepository;
        _messageRepository = messageRepository;
        _attachmentService = attachmentService;
        _userRepository = userRepository;
        _idGenerator = idGenerator;
        _gateway = gateway;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<MessageSchema>> Handle(AddOrEditMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel is null)
        {
            return ApiErrors.Channel.NotFound(request.ChannelId);
        }

        if (!channel.IsUserInTheChannel(_clientInfo.UserId))
        {
            return ApiErrors.Channel.NotAllowedToInteractWith(channel.Id);
        }

        var user = await _userRepository.GetByIdOrNullAsync(_clientInfo.UserId);
        if (user is null)
        {
            throw new ArgumentException("User was expected to be found.");
        }

        List<Attachment>? attachments = null;

        if (request.Attachments?.Count > 0)
        {
            attachments = new();

            foreach (var fileData in request.Attachments)
            {
                var attachment = await _attachmentService.ValidateAndCreateAttachmentsAsync(
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
                return ApiErrors.Channel.MessageToEditNotFound(request.MessageId.Value);
            }

            if (message.AuthorId != user.Id)
            {
                return ApiErrors.Channel.MessageWasSentByAnotherUser(request.MessageId.Value);
            }

            message.Edit(request.Content, attachments);
        }
        else
        {
            message = new Message(
                _idGenerator.CreateId(),
                request.ChannelId,
                user,
                request.Content,
                attachments);
        }
        await _messageRepository.UpsertAsync(message);

        var messageSchema = MessageSchema.From(message);

        if (request.MessageId.HasValue)
        {
            await _gateway.PublishAsync(new MessageUpdateGatewayEvent(
                messageSchema,
                channel.Members.Select(m => m.UserId.ToString()),
                channel.Type));
        }
        else
        {
            await _gateway.PublishAsync(new MessageCreateGatewayEvent(
                messageSchema,
                channel.Members.Select(m => m.UserId.ToString()),
                channel.Type));
        }

        return messageSchema;
    }
}
