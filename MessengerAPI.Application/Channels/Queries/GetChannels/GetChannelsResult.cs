using MessengerAPI.Domain.Channel;

namespace MessengerAPI.Application.Channels.Queries.GetChannels;

public record GetChannelsResult(
    List<Channel> Channels
);
