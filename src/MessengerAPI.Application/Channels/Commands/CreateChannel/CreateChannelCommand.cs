using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Channels.Commands;

public record CreateChannelCommand(
    UserId Sub,
    List<UserId> Members,
    ChannelType Type,
    string? Title
) : IRequest<ErrorOr<ChannelSchema>>;
