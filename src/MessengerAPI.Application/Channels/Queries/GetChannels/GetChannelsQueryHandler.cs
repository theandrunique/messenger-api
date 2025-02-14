using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Data.Channels;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetChannels;

public class GetChannelsQueryHandler : IRequestHandler<GetChannelsQuery, ErrorOr<List<ChannelSchema>>>
{
    private readonly IChannelRepository _channelRepository;
    private readonly IClientInfoProvider _clientInfo;

    public GetChannelsQueryHandler(IChannelRepository channelRepository, IClientInfoProvider clientInfo)
    {
        _channelRepository = channelRepository;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<List<ChannelSchema>>> Handle(GetChannelsQuery request, CancellationToken cancellationToken)
    {
        List<Channel> channels = await _channelRepository.GetUserChannelsAsync(_clientInfo.UserId);

        return ChannelSchema.From(channels);
    }
}
