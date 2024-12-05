using ErrorOr;
using MediatR;
using MessengerAPI.Application.Auth.Common;

namespace MessengerAPI.Application.Auth.Commands.Login;

/// <summary>
/// Login command
/// </summary>
/// <param name="Login">Email or username</param>
/// <param name="Password">Password</param>
public record LoginCommand(
    string Login,
    string Password,
    string CaptchaToken) : IRequest<ErrorOr<TokenPairResponse>>;
