using AutoMapper;
using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Data.Users;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Queries.GetMeQuery;

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, ErrorOr<UserPrivateSchema>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetMeQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get current user private data
    /// </summary>
    /// <param name="request"><see cref="GetMeQuery"/></param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="UserPrivateSchema"/></returns>
    public async Task<ErrorOr<UserPrivateSchema>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdOrDefaultAsync(request.Sub);

        if (user == null)
        {
            return Errors.ApiErrors.User.NotFound;
        }

        return _mapper.Map<UserPrivateSchema>(user);
    }
}
