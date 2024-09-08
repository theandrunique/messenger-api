using ErrorOr;
using MediatR;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Users.Queries;

public record GetMeQuery(UserId Sub) : IRequest<ErrorOr<User>>;
