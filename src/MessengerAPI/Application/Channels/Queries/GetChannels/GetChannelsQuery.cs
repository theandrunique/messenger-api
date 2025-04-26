using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Queries.GetChannels;

public record GetChannelsQuery() : IRequest<ErrorOr<List<ChannelSchema>>>;
