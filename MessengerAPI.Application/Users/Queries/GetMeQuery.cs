using ErrorOr;
using MediatR;
using MessengerAPI.Domain.User;
using MessengerAPI.Domain.User.ValueObjects;

namespace MessengerAPI.Application.Users.Queries;

public record GetMeQuery(UserId Sub) : IRequest<ErrorOr<User>>;
