using ErrorOr;
using MediatR;

namespace MessengerAPI.Application.Users.Commands.RecoveryPasswordComplete;
public record PasswordRecoveryCompleteCommand(string Login, string NewPassword, string Totp) : IRequest<ErrorOr<bool>>;
