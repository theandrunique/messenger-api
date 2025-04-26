using MediatR;
using Messenger.Application.Channels.Common;
using Messenger.ApiErrors;

namespace Messenger.Application.Channels.Commands.DeleteCloudAttachment;

public class DeleteCloudAttachmentCommandHandler : IRequestHandler<DeleteCloudAttachmentCommand, ErrorOr<bool>>
{
    private readonly AttachmentService _attachmentService;

    public DeleteCloudAttachmentCommandHandler(AttachmentService attachmentService)
    {
        _attachmentService = attachmentService;
    }

    public async Task<ErrorOr<bool>> Handle(DeleteCloudAttachmentCommand request, CancellationToken cancellationToken)
    {
        if (!await _attachmentService.IsObjectExistsAsync(request.uploadFilename, cancellationToken))
        {
            return Errors.Attachment.NotFoundInObjectStorage(request.uploadFilename);
        }

        var attachment = await _attachmentService.FindAttachmentByUploadFilename(request.uploadFilename);
        if (attachment is not null)
        {
            return Errors.Attachment.ObjectInUse(request.uploadFilename, attachment.Id);
        }

        await _attachmentService.DeleteObjectAsync(request.uploadFilename, cancellationToken);
        return true;
    }
}
