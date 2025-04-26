using MediatR;
using Messenger.Contracts.Common;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Queries.GetChannels;

public record GetChannelsQuery() : IRequest<ErrorOr<List<ChannelSchema>>>;
