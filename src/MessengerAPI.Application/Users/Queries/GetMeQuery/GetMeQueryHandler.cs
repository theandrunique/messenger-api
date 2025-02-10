using AutoMapper;
using MediatR;
using MessengerAPI.Application.Common.Interfaces;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Data.Users;
using MessengerAPI.Errors;

namespace MessengerAPI.Application.Users.Queries.GetMeQuery;

public class GetMeQueryHandler : IRequestHandler<GetMeQuery, ErrorOr<UserPrivateSchema>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IClientInfoProvider _clientInfo;

    public GetMeQueryHandler(IUserRepository userRepository, IMapper mapper, IClientInfoProvider clientInfo)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _clientInfo = clientInfo;
    }

    public async Task<ErrorOr<UserPrivateSchema>> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdOrNullAsync(_clientInfo.UserId);

        if (user == null)
        {
            throw new Exception("User was expected to be found here.");
        }

        return _mapper.Map<UserPrivateSchema>(user);
    }
}
