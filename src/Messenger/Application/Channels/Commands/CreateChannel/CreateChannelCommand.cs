using MediatR;
using Messenger.Contracts.Common;
using Messenger.Errors;

namespace Messenger.Application.Channels.Commands;

public record CreateChannelCommand(
    List<long> Members,
    string Name
) : IRequest<ErrorOr<ChannelSchema>>;
