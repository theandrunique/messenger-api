using MediatR;
using MessengerAPI.Application.Channels.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Channels.Commands.DeleteAttachment;

public class DeleteAttachmentCommandHandler : IRequestHandler<DeleteAttachmentCommand, ErrorOr<bool>>
{
    private readonly AttachmentService _attachmentService;

    public DeleteAttachmentCommandHandler(AttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteAttachmentCommand request, CancellationToken cancellationToken)
    {
        if (!await _attachmentService.IsObjectExistsAsync(request.uploadFilename, cancellationToken))
        {
            return ApiErrors.Attachment.NotFoundInObjectStorage(request.uploadFilename);
        }

        var attachment = await _attachmentService.FindAttachmentByUploadFilename(request.uploadFilename);
        if (attachment is not null)
        {
            return ApiErrors.Attachment.ObjectInUse(request.uploadFilename, attachment.Id);
        }

        await _attachmentService.DeleteObjectAsync(request.uploadFilename, cancellationToken);
        return true;
    }
}
