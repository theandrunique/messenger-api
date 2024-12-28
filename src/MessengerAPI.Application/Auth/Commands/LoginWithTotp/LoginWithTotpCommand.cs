using MediatR;
using MessengerAPI.Application.Auth.Common;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Auth.Commands.LoginWithTotp;

public record LoginWithTotpCommand(
    string Login,
    string Totp) : IRequest<ErrorOr<TokenPairResponse>>;
