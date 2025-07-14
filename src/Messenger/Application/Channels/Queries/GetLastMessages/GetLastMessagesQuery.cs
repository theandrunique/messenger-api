using MediatR;
using Messenger.Contracts.Common;
using Messenger.Errors;

namespace Messenger.Application.Channels.Queries.GetLastMessages;

public record GetLastMessagesQuery(List<long> ChannelIds) : IRequest<ErrorOr<Dictionary<string, MessageSchema?>>>;
