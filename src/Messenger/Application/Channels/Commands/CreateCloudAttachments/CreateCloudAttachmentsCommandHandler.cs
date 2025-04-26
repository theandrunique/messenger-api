using FluentValidation;
using MediatR;
using Messenger.Application.Channels.Common;
using Messenger.Contracts.Common;

namespace Messenger.Application.Channels.Commands.CreateCloudAttachments;

public class CreateCloudAttachmentsCommandHandler : IRequestHandler<CreateCloudAttachmentsCommand, CreateCloudAttachmentsResponse>
{
    private readonly AttachmentService _attachmentService;
    private readonly IValidator<UploadAttachmentDto> _validator;

    public CreateCloudAttachmentsCommandHandler(AttachmentService attachmentService, IValidator<UploadAttachmentDto> validator)
    {
        _attachmentService = attachmentService;
        _validator = validator;
    }

    public Task<CreateCloudAttachmentsResponse> Handle(CreateCloudAttachmentsCommand request, CancellationToken cancellationToken)
    {
        List<CloudAttachmentSchema> results = new();
        List<CloudAttachmentErrorSchema> errors = new();

        foreach (var file in request.Files)
        {
            var validationResult = _validator.Validate(file);

            if (validationResult.IsValid)
            {
                var uploadUrlDto = _attachmentService.GenerateUploadUrl(
                    size: file.FileSize,
                    channelId: request.ChannelId,
                    filename: file.Filename);

                results.Add(new CloudAttachmentSchema(
                    id: file.Id,
                    uploadFilename: uploadUrlDto.UploadFilename,
                    uploadUrl: uploadUrlDto.UploadUrl
                ));
            }
            else
            {
                errors.Add(new CloudAttachmentErrorSchema(
                    file.Id,
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }
        }

        return Task.FromResult(new CreateCloudAttachmentsResponse(results, errors));
    }
}
