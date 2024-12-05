using ErrorOr;
using MediatR;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.Users.Queries.GetMeQuery;

/// <summary>
/// Get current user private data
/// </summary>
/// <param name="Sub">User id</param>
public record GetMeQuery(Guid Sub) : IRequest<ErrorOr<UserPrivateSchema>>;

