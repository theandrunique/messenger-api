using MediatR;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Commands.DeleteCloudAttachment;

public record DeleteCloudAttachmentCommand(string uploadFilename) : IRequest<ErrorOr<bool>>;

