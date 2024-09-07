using ErrorOr;
using MediatR;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Application.Channels.Commands;

public record CreateChannelCommand(
    UserId Sub,
    List<UserId> Recipients,
    string? Title
) : IRequest<ErrorOr<CreateChannelResult>>;
