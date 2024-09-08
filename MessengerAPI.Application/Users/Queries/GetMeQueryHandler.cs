using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Domain.Common.Errors;
using MessengerAPI.Domain.User;

namespace MessengerAPI.Application.Users.Queries;

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, ErrorOr<User>>
{
    private readonly IUserRepository _userRepository;

    public GetMeQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<User>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Sub);

        if (user == null)
        {
            return UserErrors.NotFound;
        }

        return user;
    }
}
