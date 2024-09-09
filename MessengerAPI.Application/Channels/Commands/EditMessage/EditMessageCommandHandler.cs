using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.Common.Errors;

namespace MessengerAPI.Application.Channels.Commands.EditMessage;

public class EditMessageCommandHandler : IRequestHandler<EditMessageCommand, ErrorOr<Message>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IFileRepository _fileRepository;

    public EditMessageCommandHandler(IChannelRepository channelRepository, IFileRepository fileRepository)
    {
        _channelRepository = channelRepository;
        _fileRepository = fileRepository;
    }

    public async Task<ErrorOr<Message>> Handle(EditMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdAsync(request.ChannelId);
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
            attachments = await _fileRepository.GetFilesByIdsAsync(request.Attachments);
        }
        if (request.ReplyTo != null)
        {
            var replyToMessage = _channelRepository.GetMessageByIdAsync(request.ReplyTo);
            if (replyToMessage is null)
            {
                return ChannelErrors.MessageNotFound;
            }
        }

        Message? message = await _channelRepository.GetMessageByIdAsync(request.MessageId);
        if (message is null)
        {
            return ChannelErrors.MessageNotFound;
        }

        message.Update(request.ReplyTo, request.Text, attachments);

        await _channelRepository.Commit();

        return message;
    }
}
