using MediatR;
using Messenger.Application.Common.Interfaces;
using Messenger.Contracts.Common;
using Messenger.Domain.Data.Auth;
using Messenger.Errors;

namespace Messenger.Application.Users.Queries.GetMeQuery;

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, ErrorOr<UserPrivateSchema>>
{
    private readonly IUserRepository _userRepository;
    private readonly IClientInfoProvider _clientInfo;

    public GetMeQueryHandler(IUserRepository userRepository, IClientInfoProvider clientInfo)
    {
        _userRepository = userRepository;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<UserPrivateSchema>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdOrNullAsync(_clientInfo.UserId);

        if (user == null)
        {
            throw new Exception("User was expected to be found here.");
        }

        return UserPrivateSchema.From(user);
    }
}
