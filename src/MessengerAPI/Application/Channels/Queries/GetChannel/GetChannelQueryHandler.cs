using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Data.Interfaces.Channels;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetChannel;

public class GetChannelQueryHandler : IRequestHandler<GetChannelQuery, ErrorOr<ChannelSchema>>
{
    private readonly IClientInfoProvider _clientInfo;
    private readonly IChannelRepository _channelRepository;

    public GetChannelQueryHandler(IClientInfoProvider clientInfo, IChannelRepository channelRepository)
    {
        _clientInfo = clientInfo;
        _channelRepository = channelRepository;
    }

    public async Task<ErrorOr<ChannelSchema>> Handle(GetChannelQuery request, CancellationToken cancellationToken)
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

        return ChannelSchema.From(channel, _clientInfo.UserId);
    }
}
