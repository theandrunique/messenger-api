using FluentValidation;
using MessengerAPI.Application.Channels.Commands.AddOrEditMessage;
using MessengerAPI.Core;

namespace MessengerAPI.Application.Channels.Commands.AddOrUpdateMessage;

public class AddOrEditMessageCommandValidator : AbstractValidator<AddOrEditMessageCommand>
{
    public AddOrEditMessageCommandValidator()
    {
        RuleFor(x => x.Content)
            .MaximumLength(MessengerConstants.Message.MaxContentLength);

        RuleFor(x => x.Attachments)
            .Must(a => a == null || a.Count <= MessengerConstants.Message.MaxAttachmentsCount)
            .WithMessage($"Maximum allowed attachments count is {MessengerConstants.Message.MaxAttachmentsCount}.")
            .Must(attachments => attachments == null || attachments.Distinct().Count() == attachments.Count)
            .WithMessage("Attachments must be unique.");
        
        RuleFor(x => x)
            .Must(cmd => !string.IsNullOrWhiteSpace(cmd.Content) || (cmd.Attachments != null && cmd.Attachments.Any()))
            .WithMessage("Either content must be provided or at least one attachment is required.");
    }
}
