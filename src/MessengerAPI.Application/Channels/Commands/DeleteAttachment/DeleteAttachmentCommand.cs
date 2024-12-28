using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.DeleteAttachment;

public record DeleteAttachmentCommand(string uploadFilename) : IRequest<ErrorOr<bool>>;

