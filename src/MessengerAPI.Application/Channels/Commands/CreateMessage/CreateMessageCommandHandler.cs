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
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>New created message</returns>
    public async Task<ErrorOr<MessageSchema>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdAsync(request.ChannelId, cancellationToken);
        if (channel is null)
        {
            return ChannelErrors.ChannelNotFound;
        }
        if (!channel.Members.Any(m => m.Id == request.Sub))
        {
            return ChannelErrors.NotAllowed;
        }

        List<FileData>? attachments = null;

        if (request.Attachments?.Count > 0)
        {
            attachments = await _fileRepository.GetFilesByIdsAsync(request.Attachments, cancellationToken);
        }
        if (request.ReplyTo != null)
        {
            var replyToMessage = await _channelRepository.GetMessageByIdAsync(request.ReplyTo, cancellationToken);
            if (replyToMessage is null)
            {
                return ChannelErrors.MessageNotFound;
            }
        }

        Message message = channel.AddMessage(request.Sub, request.Text, request.ReplyTo, attachments);

        await _channelRepository.Commit(cancellationToken);

        return _mapper.Map<MessageSchema>(message);
    }
}
