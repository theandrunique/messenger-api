using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.PostAttachment;

public record PostAttachmentCommand(
    Guid ChannelId,
    IEnumerable<FileData> Files
) : IRequest<ErrorOr<List<CloudAttachmentSchema>>>;
