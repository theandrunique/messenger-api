using MediatR;
using Messenger.Contracts.Common;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Commands.GetDMChannel;

public record GetDMChannelCommand(
    long userId
) : IRequest<ErrorOr<ChannelSchema>>;
