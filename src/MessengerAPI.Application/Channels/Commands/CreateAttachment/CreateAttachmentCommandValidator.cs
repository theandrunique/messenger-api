using FluentValidation;
using MessengerAPI.Application.Common.Interfaces.Files;

namespace MessengerAPI.Application.Channels.Commands.CreateAttachment;

public class CreateAttachmentCommandValidator : AbstractValidator<CreateAttachmentCommand>
{
    public CreateAttachmentCommandValidator(IStorageOptions options)
    {
        RuleFor(x => x.Files)
            .NotEmpty();

        RuleForEach(x => x.Files)
            .NotEmpty()
            .Must(file => file.FileSize > 0)
            .WithMessage("File size must be greater than 0")
            .Must(file => file.FileSize < options.MaxFileSize)
            .WithMessage($"Max file size is {options.MaxFileSize} bytes");
    }
}
