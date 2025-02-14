using FluentValidation;
using MessengerAPI.Core;

namespace MessengerAPI.Application.Channels.Commands.CreateAttachment;

public class CreateAttachmentCommandValidator : AbstractValidator<CreateAttachmentCommand>
{
    public CreateAttachmentCommandValidator()
    {
        RuleFor(x => x.Files)
            .NotEmpty();

        RuleForEach(x => x.Files)
            .NotEmpty()
            .Must(file => file.FileSize > 0)
            .WithMessage("Attachment size must be greater than 0")
            .Must(file => file.FileSize < MessengerConstants.Attachment.MaxSize)
            .WithMessage($"Maximum allowed upload size is {MessengerConstants.Attachment.MaxSize} bytes");
    }
}
