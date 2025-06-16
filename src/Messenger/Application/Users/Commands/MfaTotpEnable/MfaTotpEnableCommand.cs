using MediatR;
using Messenger.Application.Users.Commands.MfaTotpEnable;
using Messenger.Errors;

namespace Messenger.Application.Users.Commands;

public record MfaTotpEnableCommand(
    string Password,
    string? EmailCode) : IRequest<ErrorOr<MfaTotpEnableCommandResult>>;
