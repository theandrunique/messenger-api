using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.SetUpTotp;

public record SetUpTotpCommand(
    Guid Sub
) : IRequest<ErrorOr<SetUpTotpCommandResponse>>;

