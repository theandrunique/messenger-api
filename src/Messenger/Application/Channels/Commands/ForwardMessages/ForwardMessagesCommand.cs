using MediatR;
using Messenger.Errors;
using Messenger.Contracts.Common;

namespace Messenger.Application.Channels.Commands.ForwardMessages;

public record ForwardMessagesCommand(
    long ChannelId,
    List<long> MessageIds,
    long TargetChannelId) : IRequest<ErrorOr<List<MessageSchema>>>;
