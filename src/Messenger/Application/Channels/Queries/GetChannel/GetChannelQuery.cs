using MediatR;
using Messenger.Contracts.Common;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Queries.GetChannel;

public record GetChannelQuery(long ChannelId) : IRequest<ErrorOr<ChannelSchema>>;
