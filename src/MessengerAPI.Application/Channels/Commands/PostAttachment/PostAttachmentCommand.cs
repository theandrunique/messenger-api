using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.PostAttachment;

public record PostAttachmentCommand(
    long ChannelId,
    IEnumerable<CreateAttachmentData> Files
) : IRequest<ErrorOr<List<CloudAttachmentSchema>>>;
