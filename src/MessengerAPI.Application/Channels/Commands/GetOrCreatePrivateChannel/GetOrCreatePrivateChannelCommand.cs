using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.GetOrCreatePrivateChannel;

public record GetOrCreatePrivateChannelCommand(
    long userId
) : IRequest<ErrorOr<ChannelSchema>>;
