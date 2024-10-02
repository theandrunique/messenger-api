using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.Common.Errors;

namespace MessengerAPI.Application.Channels.Commands.CreateMessage;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, ErrorOr<MessageSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IMapper _mapper;

    public CreateMessageCommandHandler(IChannelRepository channelRepository, IFileRepository fileRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _fileRepository = fileRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Create a new message
    /// </summary>
    /// <param name="request"><see cref="CreateMessageCommand"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="MessageSchema"/></returns>
    public async Task<ErrorOr<MessageSchema>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
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
        if (request.ReplyTo != null)
        {
            var replyToMessage = await _channelRepository.GetMessageByIdOrNullAsync(request.ReplyTo, cancellationToken);
            if (replyToMessage is null)
            {
                return Errors.Channel.MessageNotFound;
            }
        }

        Message message = channel.AddMessage(request.Sub, request.Text, request.ReplyTo, attachments);

        await _channelRepository.CommitAsync(cancellationToken);

        return _mapper.Map<MessageSchema>(message);
    }
}
