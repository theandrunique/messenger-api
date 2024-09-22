using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Users.Queries;

public record GetMeQuery(UserId Sub) : IRequest<ErrorOr<UserPrivateSchema>>;
