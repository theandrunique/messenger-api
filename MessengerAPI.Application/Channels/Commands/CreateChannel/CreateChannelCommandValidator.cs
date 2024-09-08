using FluentValidation;

namespace MessengerAPI.Application.Channels.Commands.CreateChannel;

public class CreateChannelCommandValidator : AbstractValidator<CreateChannelCommand>
{
    public CreateChannelCommandValidator()
    {
        RuleFor(x => x.Members)
            .NotEmpty()
            .WithMessage("Members cannot be empty")
            .Must(x => x.Distinct().Count() == x.Count())
            .WithMessage("Members must be unique");
    }
}
