using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.UpdateChannel;

public record UpdateChannelCommand(long ChannelId, string Title) : IRequest<ErrorOr<ChannelSchema>>;
