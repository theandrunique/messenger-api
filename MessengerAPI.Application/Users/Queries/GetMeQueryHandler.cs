using AutoMapper;
using ErrorOr;
using MediatR;
using MessengerAPI.Application.Common.Interfaces.Persistance;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.Common.Errors;

namespace MessengerAPI.Application.Users.Queries;

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, ErrorOr<UserPrivateSchema>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetMeQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<UserPrivateSchema>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Sub, cancellationToken);

        if (user == null)
        {
            return UserErrors.NotFound;
        }

        return _mapper.Map<UserPrivateSchema>(user);
    }
}
