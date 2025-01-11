using AutoMapper;
using MediatR;
using MessengerAPI.Application.Channels.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Data.Channels;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddOrUpdateMessage;

public class AddOrEditMessageCommandHandler : IRequestHandler<AddOrUpdateMessageCommand, ErrorOr<MessageSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMapper _mapper;
    private readonly IAttachmentService _attachmentService;
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public AddOrEditMessageCommandHandler(
        IChannelRepository channelRepository,
        IMapper mapper,
        IMessageRepository messageRepository,
        IAttachmentService attachmentService,
        IUserRepository userRepository)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
        _messageRepository = messageRepository;
        _attachmentService = attachmentService;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<MessageSchema>> Handle(AddOrUpdateMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel is null)
        {
            return ApiErrors.Channel.ChannelNotFound;
        }

        if (!channel.IsUserInTheChannel(request.Sub))
        {
            return ApiErrors.Channel.NotAllowed;
        }

        var user = await _userRepository.GetByIdOrDefaultAsync(request.Sub);
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
                    channel.Id,
                    cancellationToken);

                if (attachment.IsError)
                {
                    return attachment.Error;
                }

                attachments.Add(attachment.Value);
            }
        }

        if (request.ReplyTo.HasValue)
        {
            var replyToMessage = await _messageRepository.GetMessageByIdAsync(request.ChannelId, request.ReplyTo.Value);
            if (replyToMessage is null)
            {
                return ApiErrors.Channel.MessageNotFound;
            }
        }

        Message? message;

        if (request.MessageId.HasValue)
        {
            message = await _messageRepository.GetMessageByIdAsync(request.ChannelId, request.MessageId.Value);
            if (message is null)
            {
                return ApiErrors.Channel.MessageNotFound;
            }

            message.Update(request.ReplyTo, request.Content, attachments);
            await _messageRepository.RewriteAsync(message);
        }
        else
        {
            message = Message.Create(request.ChannelId, user, request.Content, request.ReplyTo, attachments);
            await _messageRepository.AddAsync(message);
        }


        return _mapper.Map<MessageSchema>(message);
    }
}

