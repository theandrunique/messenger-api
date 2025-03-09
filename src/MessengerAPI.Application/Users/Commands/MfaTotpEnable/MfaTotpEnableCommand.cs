using MediatR;
using MessengerAPI.Application.Users.Commands.MfaTotpEnable;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands;

public record MfaTotpEnableCommand(
    string Password,
    string? EmailCode) : IRequest<ErrorOr<MfaTotpEnableCommandResult>>;
