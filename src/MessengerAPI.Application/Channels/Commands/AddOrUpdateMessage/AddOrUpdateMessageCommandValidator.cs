using FluentValidation;

namespace MessengerAPI.Application.Channels.Commands.AddOrUpdateMessage;

public class AddOrUpdateMessageCommandValidator : AbstractValidator<AddOrUpdateMessageCommand>
{
    public AddOrUpdateMessageCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(10000);

        RuleFor(x => x.Attachments)
            .Must(attachments => attachments == null || attachments.Distinct().Count() == attachments.Count)
            .WithMessage("Attachments must be unique.");
    }
}
