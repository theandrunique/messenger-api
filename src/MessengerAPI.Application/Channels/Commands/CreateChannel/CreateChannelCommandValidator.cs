using FluentValidation;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;

namespace MessengerAPI.Application.Channels.Commands.CreateChannel;

public class CreateChannelCommandValidator : AbstractValidator<CreateChannelCommand>
{
    public CreateChannelCommandValidator()
    {
        RuleFor(x => x.Members)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("Members must be unique");

        RuleFor(x => x.Members)
            .Must(members => members.Count < 2)
            .When(x => x.Type == ChannelType.Private)
            .WithMessage("Members count must be less than 2 when the channel type is private");
    }
}
