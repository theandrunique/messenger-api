using Messenger.Domain.Entities;
using Messenger.Domain.Users;

namespace Messenger.Application.Users.Common;

public interface IUserSearchService
{
    Task IndexAsync(User user, CancellationToken ct);
    Task UpdateAsync(User user, CancellationToken ct);
    Task<IEnumerable<UserIndexModel>> SearchAsync(string query, CancellationToken ct);
}
