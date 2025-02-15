using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Data.Channels;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetMessages;

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, ErrorOr<List<MessageSchema>>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IClientInfoProvider _clientInfo;

    public GetMessagesQueryHandler(
        IChannelRepository channelRepository,
        IMessageRepository messageRepository,
        IClientInfoProvider clientInfo)
    {
        _channelRepository = channelRepository;
        _messageRepository = messageRepository;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<List<MessageSchema>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel is null)
        {
            return ApiErrors.Channel.NotFound(request.ChannelId);
        }
        if (!channel.HasMember(_clientInfo.UserId))
        {
            return ApiErrors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        var messages = await _messageRepository.GetMessagesAsync(request.ChannelId, request.Before, request.Limit);

        return MessageSchema.From(messages);
    }
}
