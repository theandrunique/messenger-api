using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.Common.Errors;
using MessengerAPI.Repositories.Interfaces;

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
        var user = await _userRepository.GetByIdOrNullAsync(request.Sub);

        if (user == null)
        {
            return Errors.User.NotFound;
        }

        return _mapper.Map<UserPrivateSchema>(user);
    }
}
