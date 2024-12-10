using ErrorOr;
using MediatR;
using MessengerAPI.Application.Auth.Common;

namespace MessengerAPI.Application.Auth.Commands.LoginWithTotp;

public record LoginWithTotpCommand(
    string Login,
    string Totp,
    string CaptchaToken) : IRequest<ErrorOr<TokenPairResponse>>;
