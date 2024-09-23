using ErrorOr;
using MediatR;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Users.Queries;

/// <summary>
/// Get current user private data
/// </summary>
/// <param name="Sub"><see cref="UserId"/></param>
public record GetMeQuery(UserId Sub) : IRequest<ErrorOr<UserPrivateSchema>>;
