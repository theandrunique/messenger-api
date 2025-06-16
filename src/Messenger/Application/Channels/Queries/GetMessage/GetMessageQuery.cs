using MediatR;
using Messenger.Errors;
using Messenger.Contracts.Common;

namespace Messenger.Application.Channels.Queries.GetMessage;

public record GetMessageQuery(long ChannelId, long MessageId) : IRequest<ErrorOr<MessageSchema>>;
