using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Data.Channels;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.MessageAck;

public class MessageAckCommandHandler : IRequestHandler<MessageAckCommand, ErrorOr<Unit>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IClientInfoProvider _clientInfo;

    public MessageAckCommandHandler(
        IChannelRepository channelRepository,
        IClientInfoProvider clientInfo,
        IMessageRepository messageRepository)
    {
        _channelRepository = channelRepository;
        _clientInfo = clientInfo;
        _messageRepository = messageRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(MessageAckCommand request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel == null)
        {
            return ApiErrors.Channel.NotFound(request.ChannelId);
        }
        if (!channel.HasMember(_clientInfo.UserId))
        {
            return ApiErrors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        var memberInfo = channel.Members.First(m => m.UserId == _clientInfo.UserId);

        if (memberInfo.ReadAt >= request.MessageId)
        {
            return Unit.Value;
        }

        var message = await _messageRepository.GetMessageByIdOrNullAsync(request.ChannelId, request.MessageId);
        if (message == null)
        {
            return ApiErrors.Channel.MessageNotFound(request.MessageId);
        }

        await _channelRepository.UpdateReadAt(_clientInfo.UserId, request.ChannelId, request.MessageId);

        return Unit.Value;
    }
}
