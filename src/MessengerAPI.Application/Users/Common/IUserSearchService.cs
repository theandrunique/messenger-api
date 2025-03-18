using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.Users;

namespace MessengerAPI.Application.Users.Common;

public interface IUserSearchService
{
    public Task IndexAsync(User user, CancellationToken ct);
    public Task<IEnumerable<UserIndexModel>> SearchAsync(string query, CancellationToken ct);
}
