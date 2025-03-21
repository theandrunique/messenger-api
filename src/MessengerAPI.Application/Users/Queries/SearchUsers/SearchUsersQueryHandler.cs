using MediatR;
using MessengerAPI.Application.Users.Common;
using MessengerAPI.Contracts.Common;

namespace MessengerAPI.Application.Users.Queries.SearchUsers;

public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, List<UserPublicSchema>>
{
    private readonly IUserSearchService _searchService;

    public SearchUsersQueryHandler(IUserSearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<List<UserPublicSchema>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _searchService.SearchAsync(request.Query, cancellationToken);
        return result.Select(UserPublicSchema.From).ToList();
    }
}