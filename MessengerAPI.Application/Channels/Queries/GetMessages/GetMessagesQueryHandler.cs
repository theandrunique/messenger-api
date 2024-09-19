using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Application.Schemas.Common;
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

    public async Task<ErrorOr<List<MessageSchema>>> Handle(GetMessagesQuery request, CancellationToken cancellationToken)
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

        var messages = await _channelRepository.GetMessagesAsync(request.ChannelId, request.Limit, request.Offset, cancellationToken);

        return _mapper.Map<List<MessageSchema>>(messages);
    }
}
