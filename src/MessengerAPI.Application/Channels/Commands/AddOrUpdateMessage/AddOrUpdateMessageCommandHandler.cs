using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Files;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Common.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddOrUpdateMessage;

public class AddOrEditMessageCommandHandler : IRequestHandler<AddOrUpdateMessageCommand, ErrorOr<MessageSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorage;

    public AddOrEditMessageCommandHandler(IChannelRepository channelRepository, IMapper mapper, IFileStorageService fileStorage)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
        _fileStorage = fileStorage;
    }

    /// <summary>
    /// Creates new or updates existing message 
    /// </summary>
    /// <param name="request"><see cref="AddOrUpdateMessageCommand"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="MessageSchema"/></returns>
    public async Task<ErrorOr<MessageSchema>> Handle(AddOrUpdateMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId, cancellationToken);
        if (channel is null) return Errors.Channel.ChannelNotFound;
        if (!channel.CanUserAccessChannel(request.Sub)) return Errors.Channel.NotAllowed;

        List<Attachment>? attachments = null;

        if (request.Attachments?.Count > 0)
        {
            attachments = new();

            foreach (var fileData in request.Attachments)
            {
                var objectMetadata = await _fileStorage.GetObjectMetadataAsync(fileData.UploadedFilename, cancellationToken);
                if (objectMetadata is null) return Errors.File.NotFound(fileData.UploadedFilename);

                var preSignedUrlExpiresAt = DateTime.UtcNow.AddDays(7);

                var preSignedUrl = await _fileStorage.GeneratePreSignedUrlForDownloadAsync(
                    fileData.UploadedFilename,
                    preSignedUrlExpiresAt);

                var attachment = Attachment.Create(
                    channel.Id,
                    fileData.Filename,
                    fileData.UploadedFilename,
                    objectMetadata.ContentType,
                    objectMetadata.ObjectSize,
                    preSignedUrl,
                    preSignedUrlExpiresAt
                );
                attachments.Add(attachment);
            }
            await _channelRepository.AddAttachmentsAsync(attachments, cancellationToken);
        }

        if (request.ReplyTo.HasValue)
        {
            var replyToMessage = await _channelRepository.GetMessageByIdOrNullAsync(request.ReplyTo.Value, cancellationToken);
            if (replyToMessage is null)
            {
                return Errors.Channel.MessageNotFound;
            }
        }

        Message? message;

        if (request.MessageId.HasValue)
        {
            message = await _channelRepository.GetMessageByIdOrNullAsync(request.MessageId.Value, cancellationToken);
            if (message is null)
            {
                return Errors.Channel.MessageNotFound;
            }

            message.Update(request.ReplyTo, request.Text, attachments);
        }
        else
        {
            message = channel.AddMessage(request.Sub, request.Text, request.ReplyTo, attachments);
        }

        await _channelRepository.UpdateAsync(channel, cancellationToken);

        return _mapper.Map<MessageSchema>(message);
    }
}
