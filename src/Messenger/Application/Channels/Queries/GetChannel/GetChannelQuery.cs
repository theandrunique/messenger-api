using MediatR;
using Messenger.Contracts.Common;
using Messenger.Errors;

namespace Messenger.Application.Channels.Queries.GetChannel;

public record GetChannelQuery(long ChannelId) : IRequest<ErrorOr<ChannelSchema>>;
