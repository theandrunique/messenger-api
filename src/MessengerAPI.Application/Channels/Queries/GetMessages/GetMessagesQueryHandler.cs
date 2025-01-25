using AutoMapper;
using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Data.Channels;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetMessages;

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, ErrorOr<List<MessageSchema>>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;

    public GetMessagesQueryHandler(IChannelRepository channelRepository, IMapper mapper, IMessageRepository messageRepository)
    {
        _channelRepository = channelRepository;
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<List<MessageSchema>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel is null)
        {
            return ApiErrors.Channel.NotFound(request.ChannelId);
        }
        if (!channel.IsUserInTheChannel(request.Sub))
        {
            return ApiErrors.Channel.NotAllowedToInteractWith(channel.Id);
        }

        var messages = await _messageRepository.GetMessagesAsync(request.ChannelId, request.Before, request.Limit);

        return _mapper.Map<List<MessageSchema>>(messages);
    }
}
