using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.Common.Errors;

namespace MessengerAPI.Application.Channels.Commands.AddOrUpdateMessage;

public class AddOrUpdateMessageCommandHandler : IRequestHandler<AddOrUpdateMessageCommand, ErrorOr<MessageSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IMapper _mapper;

    public AddOrUpdateMessageCommandHandler(IChannelRepository channelRepository, IFileRepository fileRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _fileRepository = fileRepository;
        _mapper = mapper;
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
        if (channel is null)
        {
            return Errors.Channel.ChannelNotFound;
        }
        if (!channel.Members.Any(m => m.Id == request.Sub))
        {
            return Errors.Channel.NotAllowed;
        }

        List<FileData>? attachments = null;

        if (request.Attachments?.Count > 0)
        {
            attachments = await _fileRepository.GetFilesByIdsAsync(request.Attachments, cancellationToken);
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

