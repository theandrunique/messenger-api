using ErrorOr;
using MediatR;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.Channels.Queries.GetChannels;

/// <summary>
/// Get user's channels query
/// </summary>
/// <param name="Sub">User id</param>
public record GetChannelsQuery(
    Guid Sub
) : IRequest<ErrorOr<List<ChannelSchema>>>;
