using MediatR;
using Messenger.Application.Users.Common;
using Messenger.Contracts.Common;

namespace Messenger.Application.Users.Queries.SearchUsers;

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