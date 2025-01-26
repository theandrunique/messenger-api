using AutoMapper;
using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Core;
using MessengerAPI.Data.Channels;
using MessengerAPI.Data.Users;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddOrEditMessage;

public class AddOrEditMessageCommandHandler : IRequestHandler<AddOrEditMessageCommand, ErrorOr<MessageSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMapper _mapper;
    private readonly AttachmentService _attachmentService;
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IIdGenerator _idGenerator;

    public AddOrEditMessageCommandHandler(
        IChannelRepository channelRepository,
        IMapper mapper,
        IMessageRepository messageRepository,
        AttachmentService attachmentService,
        IUserRepository userRepository,
        IIdGenerator idGenerator)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
        _messageRepository = messageRepository;
        _attachmentService = attachmentService;
        _userRepository = userRepository;
        _idGenerator = idGenerator;
    }

    public async Task<ErrorOr<MessageSchema>> Handle(AddOrEditMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel is null)
        {
            return ApiErrors.Channel.NotFound(request.ChannelId);
        }

        if (!channel.IsUserInTheChannel(request.Sub))
        {
            return ApiErrors.Channel.NotAllowedToInteractWith(channel.Id);
        }

        var user = await _userRepository.GetByIdOrNullAsync(request.Sub);
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

        return _mapper.Map<MessageSchema>(message);
    }
}
