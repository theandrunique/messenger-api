using FluentValidation;
using Messenger.Application.Common.Interfaces;
using Messenger.Core;

namespace Messenger.Application.Users.Commands.UpdateAvatar;

public class UpdateAvatarCommandValidator : AbstractValidator<UpdateAvatarCommand>
{
    private static readonly string[] _allowedMimeTypes =
    {
        "image/png",
        "image/jpeg",
        "image/gif",
        "image/webp"
    };

    public UpdateAvatarCommandValidator(IImageProcessor imageProcessor)
    {
        RuleFor(x => x.File)
            .NotEmpty()
            .Must(f => _allowedMimeTypes.Contains(f.ContentType.ToLower()))
            .WithMessage($"Allowed mime types are: {string.Join(", ", _allowedMimeTypes)}")
            .Must(f => imageProcessor.IsValidImageSignature(f))
            .WithMessage("Invalid image signature");

        RuleFor(x => x.File.Length)
            .LessThan(MessengerConstants.Images.MaxSize)
            .WithMessage($"Maximum allowed image size is {MessengerConstants.Images.MaxSize / 1024 / 1024} MB");
    }
}
