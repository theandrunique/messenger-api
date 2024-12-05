using System.Net.NetworkInformation;
using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.Common.Errors;
using MessengerAPI.Repositories.Interfaces;

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

    /// <summary>
    /// Get messages from requested channel
    /// </summary>
    /// <param name="request"><see cref="GetMessagesQuery"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>A list of messages <see cref="MessageSchema"/></returns>
    public async Task<ErrorOr<List<MessageSchema>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
    {
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId);
        if (channel is null)
        {
            return Errors.Channel.ChannelNotFound;
        }
        if (!channel.IsUserInTheChannel(request.Sub))
        {
            return Errors.Channel.NotAllowed;
        }

        var messages = await _messageRepository.GetMessagesAsync(request.ChannelId, request.Limit, Guid.Empty);

        return _mapper.Map<List<MessageSchema>>(messages);
    }
}
