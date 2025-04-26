using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.GetDMChannel;

public record GetDMChannelCommand(
    long userId
) : IRequest<ErrorOr<ChannelSchema>>;
