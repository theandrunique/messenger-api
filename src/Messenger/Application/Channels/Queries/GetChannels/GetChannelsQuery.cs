using MediatR;
using Messenger.Contracts.Common;
using Messenger.Errors;

namespace Messenger.Application.Channels.Queries.GetChannels;

public record GetChannelsQuery() : IRequest<ErrorOr<List<ChannelSchema>>>;
