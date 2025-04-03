using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.DeleteCloudAttachment;

public record DeleteCloudAttachmentCommand(string uploadFilename) : IRequest<ErrorOr<bool>>;

