using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.ChannelAggregate;

namespace MessengerAPI.Application.Channels.Queries.GetChannels;

public class GetChannelsQueryHandler : IRequestHandler<GetChannelsQuery, ErrorOr<List<Channel>>>
{
    IChannelRepository _channelRepository;

    public GetChannelsQueryHandler(IChannelRepository channelRepository)
    {
        _channelRepository = channelRepository;
    }

    public async Task<ErrorOr<List<Channel>>> Handle(GetChannelsQuery request, CancellationToken cancellationToken)
    {
        List<Channel> channels = await _channelRepository.GetChannelsByUserIdAsync(request.Sub);
        return channels;
    }
}
