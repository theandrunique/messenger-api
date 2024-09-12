using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Channels.Commands;

public record CreateChannelCommand(
    UserId Sub,
    List<UserId> Members,
    string? Title
) : IRequest<ErrorOr<ChannelSchema>>;
