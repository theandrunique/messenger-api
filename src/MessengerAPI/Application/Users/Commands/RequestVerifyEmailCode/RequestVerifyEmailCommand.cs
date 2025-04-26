using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Commands.RequestVerifyEmailCode;

public record RequestVerifyEmailCommand : IRequest<ErrorOr<Unit>>;
