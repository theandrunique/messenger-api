using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Domain.Channels;
using Messenger.Domain.Channels.ValueObjects;
using Messenger.Domain.Data.Channels;
using Messenger.Errors;

namespace Messenger.Application.Channels.Queries.GetChannels;

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

        channels = channels
            .Where(c => c.HasMember(_clientInfo.UserId))
            .Where(c => !(c.Type == ChannelType.DM && c.LastMessage == null))
            .ToList();

        return ChannelSchema.From(channels, _clientInfo.UserId);
    }
}
