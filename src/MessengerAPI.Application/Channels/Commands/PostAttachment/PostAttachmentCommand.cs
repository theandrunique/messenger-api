using ErrorOr;
using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.Channels.Commands.PostAttachment;

public record PostAttachmentCommand(
    IEnumerable<FileData> Files
) : IRequest<ErrorOr<List<CloudAttachmentSchema>>>;
