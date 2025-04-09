using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands;

public record CreateChannelCommand(
    List<long> Members,
    string Name
) : IRequest<ErrorOr<ChannelSchema>>;
