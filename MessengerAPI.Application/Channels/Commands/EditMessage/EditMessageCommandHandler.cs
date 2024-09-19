using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.Common.Errors;

namespace MessengerAPI.Application.Channels.Commands.EditMessage;

public class EditMessageCommandHandler : IRequestHandler<EditMessageCommand, ErrorOr<MessageSchema>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IMapper _mapper;

    public EditMessageCommandHandler(IChannelRepository channelRepository, IFileRepository fileRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _fileRepository = fileRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<MessageSchema>> Handle(EditMessageCommand request, CancellationToken cancellationToken)
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

        Message? message = await _channelRepository.GetMessageByIdAsync(request.MessageId, cancellationToken);
        if (message is null)
        {
            return ChannelErrors.MessageNotFound;
        }

        message.Update(request.ReplyTo, request.Text, attachments);

        await _channelRepository.Commit(cancellationToken);

        return _mapper.Map<MessageSchema>(message);
    }
}
