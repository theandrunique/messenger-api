using FluentValidation;

namespace Messenger.Application.Channels.Commands.CreateChannel;

public class CreateChannelCommandValidator : AbstractValidator<CreateChannelCommand>
{
    public CreateChannelCommandValidator()
    {
        RuleFor(x => x.Members)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("Members must be unique.");
    }
}
