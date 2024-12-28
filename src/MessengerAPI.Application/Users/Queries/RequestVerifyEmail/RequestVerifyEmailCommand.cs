using MediatR;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Queries.RequestVerifyEmail;

public record RequestVerifyEmailCommand(Guid Sub) : IRequest<ErrorOr<bool>>;
