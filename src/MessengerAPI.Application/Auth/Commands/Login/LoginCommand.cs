using ErrorOr;
using MediatR;
using MessengerAPI.Application.Auth.Common;

namespace MessengerAPI.Application.Auth.Commands.Login;

/// <summary>
/// Login command
/// </summary>
/// <param name="Login">Email or username</param>
/// <param name="Password">Password</param>
/// <param name="UserAgent">User agent</param>
/// <param name="IpAddress">Ip address</param>
public record LoginCommand(
    string Login,
    string Password,
    string UserAgent,
    string IpAddress) : IRequest<ErrorOr<TokenPairResponse>>;
