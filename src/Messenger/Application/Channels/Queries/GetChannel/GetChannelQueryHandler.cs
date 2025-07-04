using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Data.Interfaces.Channels;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Queries.GetChannel;

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
            return Errors.Channel.NotFound(request.ChannelId);
        }
        if (!channel.HasMember(_clientInfo.UserId))
        {
            return Errors.Channel.UserNotMember(_clientInfo.UserId, channel.Id);
        }

        return ChannelSchema.From(channel, _clientInfo.UserId);
    }
}
