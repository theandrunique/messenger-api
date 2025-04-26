using MediatR;
using Messenger.Contracts.Common;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Commands.UpdateChannel;

public record UpdateChannelCommand(long ChannelId, string Name) : IRequest<ErrorOr<ChannelSchema>>;
