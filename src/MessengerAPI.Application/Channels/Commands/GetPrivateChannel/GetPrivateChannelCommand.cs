using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.GetPrivateChannel;

public record GetPrivateChannelCommand(
    long userId
) : IRequest<ErrorOr<ChannelSchema>>;
