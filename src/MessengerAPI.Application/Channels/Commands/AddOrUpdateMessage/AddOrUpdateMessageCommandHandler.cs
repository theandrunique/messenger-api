using AutoMapper;
using MediatR;
using MessengerAPI.Application.Channels.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Data.Channels;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddOrUpdateMessage;

public class AddOrEditMessageCommandHandler : IRequestHandler<AddOrUpdateMessageCommand, ErrorOr<MessageSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMapper _mapper;
    private readonly IAttachmentService _attachmentService;

    public AddOrEditMessageCommandHandler(
        IChannelRepository channelRepository,
        IMapper mapper,
        IAttachmentService attachmentService)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
        _attachmentService = attachmentService;
    }

    /// <summary>
    /// Creates new or updates existing message 
    /// </summary>
    /// <param name="request"><see cref="AddOrUpdateMessageCommand"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="MessageSchema"/></returns>
    public async Task<ErrorOr<MessageSchema>> Handle(AddOrUpdateMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel is null) return Error.Channel.ChannelNotFound;

        if (!channel.IsUserInTheChannel(request.Sub)) return Error.Channel.NotAllowed;

        throw new NotImplementedException();

        // List<Attachment>? attachments = null;

        // if (request.Attachments?.Count > 0)
        // {
        // attachments = new();

        // foreach (var fileData in request.Attachments)
        // {
        // var attachment = await _attachmentService.ValidateAndCreateAttachmentsAsync(
        // fileData.UploadedFilename,
        // fileData.Filename,
        // channel.Id,
        // cancellationToken);
        // if (attachment.IsError) return attachment.Errors;

        // attachments.Add(attachment.Value);
        // }
        // await _channelRepository.AddAttachmentsAsync(attachments, cancellationToken);
        // }

        // if (request.ReplyTo.HasValue)
        // {
        // var replyToMessage = await _channelRepository.GetMessageByIdOrNullAsync(request.ReplyTo.Value, cancellationToken);
        // if (replyToMessage is null)
        // {
        // return Errors.Channel.MessageNotFound;
        // }
        // }

        // Message? message;

        // if (request.MessageId.HasValue)
        // {
        // message = await _channelRepository.GetMessageByIdOrNullAsync(request.MessageId.Value, cancellationToken);
        // if (message is null)
        // {
        // return Errors.Channel.MessageNotFound;
        // }

        // message.Update(request.ReplyTo, request.Text, attachments);
        // }
        // else
        // {
        // message = channel.AddMessage(request.Sub, request.Text, request.ReplyTo, attachments);
        // }

        // await _channelRepository.UpdateAsync(channel, cancellationToken);

        // return _mapper.Map<MessageSchema>(message);
    }
}

