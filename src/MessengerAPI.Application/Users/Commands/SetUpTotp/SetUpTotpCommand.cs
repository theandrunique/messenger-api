using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.SetUpTotp;

public record SetUpTotpCommand(
    long Sub
) : IRequest<ErrorOr<SetUpTotpCommandResponse>>;

