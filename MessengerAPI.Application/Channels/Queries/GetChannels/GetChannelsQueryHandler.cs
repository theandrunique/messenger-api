using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Channel;
using MessengerAPI.Domain.Common.ValueObjects;

namespace MessengerAPI.Application.Channels.Queries.GetChannels;

public class GetChannelsQueryHandler : IRequestHandler<GetChannelsQuery, ErrorOr<GetChannelsResult>>
{
    IChannelRepository _channelRepository;

    public GetChannelsQueryHandler(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public async Task<ErrorOr<GetChannelsResult>> Handle(GetChannelsQuery request, CancellationToken cancellationToken)
    {
        List<Channel> channels = await _channelRepository.GetChannelsByUserIdAsync(request.Sub);
        return new GetChannelsResult(channels);
    }
}
