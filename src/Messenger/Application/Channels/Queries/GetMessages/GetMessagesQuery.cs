using MediatR;
using Messenger.Contracts.Common;
using Messenger.Errors;

namespace Messenger.Application.Channels.Queries.GetMessages;

public record GetMessagesQuery(
    long ChannelId,
    long Before,
    int Limit
) : IRequest<ErrorOr<List<MessageSchema>>>;
