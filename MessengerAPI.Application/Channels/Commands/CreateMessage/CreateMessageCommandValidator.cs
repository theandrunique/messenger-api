using FluentValidation;

namespace MessengerAPI.Application.Channels.Commands.CreateMessage;

public class CreateMessageCommandValidator : AbstractValidator<CreateMessageCommand>
{
    public CreateMessageCommandValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(10000);
        
        RuleFor(x => x.Attachments)
            .Must(attachments => attachments == null || attachments.Distinct().Count() == attachments.Count)
            .WithMessage("Attachments must be unique.");

    }
}
