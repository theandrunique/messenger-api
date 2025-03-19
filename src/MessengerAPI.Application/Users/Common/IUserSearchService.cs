using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.Users;

namespace MessengerAPI.Application.Users.Common;

public interface IUserSearchService
{
    Task IndexAsync(User user, CancellationToken ct);
    Task UpdateAsync(User user, CancellationToken ct);
    Task<IEnumerable<UserIndexModel>> SearchAsync(string query, CancellationToken ct);
}
