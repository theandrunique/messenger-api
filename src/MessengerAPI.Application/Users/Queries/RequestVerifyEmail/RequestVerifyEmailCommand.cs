using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Queries.RequestVerifyEmail;

public record RequestVerifyEmailCommand(long Sub) : IRequest<ErrorOr<bool>>;
