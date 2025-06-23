using MediatR;
using Messenger.Contracts.Common;
using Messenger.Domain.Data.Auth;
using Messenger.Errors;

namespace Messenger.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ErrorOr<UserPublicSchema>>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<UserPublicSchema>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdOrNullAsync(request.UserId);

        if (user == null)
        {
            return Error.User.NotFound(request.UserId);
        }

        return UserPublicSchema.From(user);
    }
}
