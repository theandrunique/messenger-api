using ErrorOr;
using MediatR;

namespace MessengerAPI.Application.Channels.Commands.DeleteAttachment;

public record DeleteAttachmentCommand(string uploadFilename) : IRequest<ErrorOr<bool>>;

