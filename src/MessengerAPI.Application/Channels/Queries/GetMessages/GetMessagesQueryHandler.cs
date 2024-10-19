using System.Text.Json;
using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.Common.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetMessages;

public class GetMessagesQueryHandler : IRequestHandler<GetMessagesQuery, ErrorOr<List<MessageSchema>>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IMapper _mapper;

    public GetMessagesQueryHandler(IChannelRepository channelRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
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
        var channel = await _channelRepository.GetByIdOrNullAsync(request.ChannelId, cancellationToken);
        if (channel is null)
        {
            return Errors.Channel.ChannelNotFound;
        }
        if (!channel.CanUserAccessChannel(request.Sub))
        {
            return Errors.Channel.NotAllowed;
        }

        var messages = await _channelRepository.GetMessagesAsync(request.ChannelId, request.Limit, request.Offset, cancellationToken);

        return _mapper.Map<List<MessageSchema>>(messages);
    }
}
