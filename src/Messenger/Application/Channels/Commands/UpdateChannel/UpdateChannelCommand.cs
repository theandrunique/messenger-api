using MediatR;
using Messenger.Contracts.Common;
using Messenger.Errors;
using Microsoft.AspNetCore.Http;

namespace Messenger.Application.Channels.Commands.UpdateChannel;

public record UpdateChannelCommand(
    long ChannelId,
    string? Name,
    IFormFile? Image) : IRequest<ErrorOr<ChannelSchema>>;
