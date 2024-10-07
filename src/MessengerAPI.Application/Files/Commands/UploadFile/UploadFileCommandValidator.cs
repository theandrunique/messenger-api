using FluentValidation;
using MessengerAPI.Application.Common.Interfaces.FileStorage;

namespace MessengerAPI.Application.Files.Commands.UploadFile;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator(IStorageOptions settings)
    {
        RuleFor(f => f.FileStream.Length).LessThanOrEqualTo(settings.MaxFileSize);
    }
}
