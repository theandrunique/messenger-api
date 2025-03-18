using Elastic.Clients.Elasticsearch;
using MessengerAPI.Application.Users.Common;
using MessengerAPI.Domain.Entities;
using MessengerAPI.Domain.Users;
using Newtonsoft.Json;

namespace MessengerAPI.Infrastructure.Users;

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
            Id = user.Id,
            GlobalName = user.GlobalName,
            Username = user.Username,
        };

        await _client.IndexAsync(model, ct);
    }

    public async Task<IEnumerable<UserIndexModel>> SearchAsync(string query, CancellationToken ct)
    {
        var response = await _client.SearchAsync<UserIndexModel>("users", s => s
            .Query(q => q.Bool(b => b
                .Should(s => s
                    .Term(t => t.Field(f => f.Username.Suffix("keyword")).Value(query).Boost(4))
                    .Match(m => m.Field(f => f.Username).Query(query).Boost(3))
                    .Wildcard(w => w.Field(f => f.Username.Suffix("keyword")).Value($"*{query}*").Boost(2))
                    .Match(m => m.Field(f => f.Username).Query(query).Fuzziness(new Fuzziness("AUTO")).Boost(1)))
                .MinimumShouldMatch(1)
        )), ct);

        Console.WriteLine($"Result: {JsonConvert.SerializeObject(response)}");

        return response.Hits.Select(h => h.Source);
    }
}
