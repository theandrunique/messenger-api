using FluentValidation;
using MessengerAPI.Application.Common.Interfaces.Files;

namespace MessengerAPI.Application.Channels.Commands.PostAttachment;

public class PostAttachmentCommandValidator : AbstractValidator<PostAttachmentCommand>
{
    public PostAttachmentCommandValidator(IStorageOptions options)
    {
        RuleFor(x => x.Files)
            .NotEmpty();

        RuleForEach(x => x.Files)
            .NotEmpty()
            .Must(file => file.Size > 0)
            .WithMessage("File size must be greater than 0")
            .Must(file => file.Size < options.MaxFileSize)
            .WithMessage($"Max file size is {options.MaxFileSize} bytes");
    }
}
