using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Auth.Commands.Login;

public record LoginCommand(
    string Login,
    string Password,
    string? Totp) : IRequest<ErrorOr<TokenPairResponse>>;
