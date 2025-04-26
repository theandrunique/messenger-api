using MediatR;
using Messenger.Application.Users.Commands.MfaTotpEnable;
using Messenger.ApiErrors;

namespace Messenger.Application.Users.Commands;

public record MfaTotpEnableCommand(
    string Password,
    string? EmailCode) : IRequest<ErrorOr<MfaTotpEnableCommandResult>>;
