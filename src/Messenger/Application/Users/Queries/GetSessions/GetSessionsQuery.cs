using MediatR;
using Messenger.Contracts.Common;

namespace Messenger.Application.Users.Queries.GetSessions;

public record GetSessionsQuery : IRequest<List<SessionSchema>>;
