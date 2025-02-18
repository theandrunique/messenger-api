using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands;

public record MfaTotpEnableCommand : IRequest<ErrorOr<Unit>>;
