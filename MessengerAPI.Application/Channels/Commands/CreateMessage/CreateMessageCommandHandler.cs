using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Channel.Entities;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.Common.Errors;

namespace MessengerAPI.Application.Channels.Commands.CreateMessage;

public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, ErrorOr<CreateMessageResult>>
{
    readonly IChannelRepository _channelRepository;
    readonly IFileRepository _fileRepository;

    public CreateMessageCommandHandler(IChannelRepository channelRepository, IFileRepository fileRepository)
    {
        _channelRepository = channelRepository;
        _fileRepository = fileRepository;
    }

    public async Task<ErrorOr<CreateMessageResult>> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdAsync(request.ChannelId);
        if (channel is null)
        {
            return ChannelErrors.ChannelNotFound;
        }
        List<FileData>? attachments = null;

        if (request.FileIds.Count > 0)
        {
            attachments = await _fileRepository.GetFilesByIdsAsync(request.FileIds);
        }

        Message message = channel.AddMessage(request.Sub, request.Text, request.ReplyTo, attachments);

        await _channelRepository.Commit();

        return new CreateMessageResult(message);
    }
}
