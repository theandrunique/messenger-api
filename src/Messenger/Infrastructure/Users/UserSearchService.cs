using Elastic.Clients.Elasticsearch;
using Messenger.Application.Users.Common;
using Messenger.Domain.Auth;
using Messenger.Domain.Users;

namespace Messenger.Infrastructure.Users;

public class UserSearchService : IUserSearchService
{
    private readonly ElasticsearchClient _client;

    public UserSearchService(ElasticsearchClient client)
    {
        _client = client;
    }

    public async Task IndexAsync(User user, CancellationToken ct)
    {
        var model = new UserIndexModel
        {
            Id = user.Id.ToString(),
            GlobalName = user.GlobalName,
            Username = user.Username,
            Image = user.Image,
        };

        await _client.IndexAsync(model, idx => idx.Index("users").Id(user.Id.ToString()), ct);
    }

    public Task UpdateAsync(User user, CancellationToken ct)
    {
        var model = new UserIndexModel
        {
            Id = user.Id.ToString(),
            GlobalName = user.GlobalName,
            Username = user.Username,
            Image = user.Image,
        };

        return _client.UpdateAsync<UserIndexModel, UserIndexModel>(
            "users",
            user.Id.ToString(),
            u => u.Doc(model),
            ct);
    }

    public async Task<IEnumerable<UserIndexModel>> SearchAsync(string query, CancellationToken ct)
    {
        var response = await _client.SearchAsync<UserIndexModel>(s => s
            .Index("users")
            .Query(q => q.Bool(b => b
                .Should(
                    s => s.Term(t => t
                        .Field(f => f.Username)
                        .Value(query)
                        .Boost(4)
                    ),
                    s => s.Match(m => m
                        .Field(f => f.Username)
                        .Query(query)
                        .Boost(3)
                    ),
                    s => s.Wildcard(w => w
                        .Field(f => f.Username)
                        .Value($"*{query}*")
                        .Boost(2)
                    ),
                    s => s.Match(m => m
                        .Field(f => f.Username)
                        .Query(query)
                        .Fuzziness(new Fuzziness("AUTO"))
                    )
                )
                .MinimumShouldMatch(1)
            ))
        );

        return response.Hits.Select(h => h.Source);
    }
}
