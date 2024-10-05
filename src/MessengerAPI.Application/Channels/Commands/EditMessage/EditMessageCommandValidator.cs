using FluentValidation;

namespace MessengerAPI.Application.Channels.Commands.EditMessage;

public class EditMessageCommandValidator : AbstractValidator<EditMessageCommand>
{
    public EditMessageCommandValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(10000);

        RuleFor(x => x.Attachments)
            .Must(attachments => attachments == null || attachments.Distinct().Count() == attachments.Count)
            .WithMessage("Attachments must be unique.");

        RuleFor(x => x.ReplyTo)
            .Must((model, field) => field.HasValue || field < model.MessageId)
            .WithMessage("ReplyTo must be older than the current MessageId.");
    }
}
