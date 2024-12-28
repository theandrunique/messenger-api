using MediatR;
using MessengerAPI.Application.Channels.Common.Interfaces;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.DeleteAttachment;

public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand, ErrorOr<bool>>
{
    private readonly IAttachmentService _attachmentService;

    public DeleteAttachmentCommandHandler(IAttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
    {
        var result = await _attachmentService.DeleteAttachmentAsync(request.uploadFilename, cancellationToken);
        if (result.IsError)
        {
            return result.Error;
        }
        return true;
    }
}

