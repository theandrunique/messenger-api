using FluentValidation;
using Messenger.Application.Channels.Common;
using Messenger.Core;

namespace Messenger.Application.Channels.Commands.CreateCloudAttachments;

public class UploadAttachmentDtoValidator : AbstractValidator<UploadAttachmentDto>
{
    public UploadAttachmentDtoValidator()
    {
        RuleFor(x => x.Filename)
            .NotEmpty();

        RuleFor(x => x.FileSize)
            .Must(size => size > 0)
            .WithMessage("Attachment size cannot be less than 0.")
            .Must(size => size <= MessengerConstants.Attachment.MaxSize)
            .WithMessage($"Maximum allowed size is {MessengerConstants.Attachment.MaxSize / 1024 / 1024} MB.");
    }
}
