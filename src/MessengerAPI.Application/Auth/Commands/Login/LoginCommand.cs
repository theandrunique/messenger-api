using ErrorOr;
using MediatR;
using MessengerAPI.Application.Auth.Common;

namespace MessengerAPI.Application.Auth.Commands.Login;

public record LoginCommand(
    string Login,
    string Password,
    string UserAgent,
    string IpAddress) : IRequest<ErrorOr<TokenPairResponse>>;
