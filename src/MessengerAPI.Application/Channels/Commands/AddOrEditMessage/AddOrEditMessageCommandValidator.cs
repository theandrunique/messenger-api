using FluentValidation;
using MessengerAPI.Application.Channels.Commands.AddOrEditMessage;

namespace MessengerAPI.Application.Channels.Commands.AddOrUpdateMessage;

public class AddOrEditMessageCommandValidator : AbstractValidator<AddOrEditMessageCommand>
{
    public AddOrEditMessageCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(10000);

        RuleFor(x => x.Attachments)
            .Must(attachments => attachments == null || attachments.Distinct().Count() == attachments.Count)
            .WithMessage("Attachments must be unique.");
    }
}
