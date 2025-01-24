using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.Models.ValueObjects;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands;

public record CreateChannelCommand(
    long Sub,
    List<long> Members,
    string? Title
) : IRequest<ErrorOr<ChannelSchema>>;
