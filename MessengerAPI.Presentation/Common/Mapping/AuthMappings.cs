using AutoMapper;
using MessengerAPI.Application.Auth.Commands.Register;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.User;
using MessengerAPI.Domain.User.ValueObjects;
using MessengerAPI.Presentation.Schemas.Common;

namespace MessengerAPI.Presentation.Common.Mapping;

public class AuthMappings : Profile
{
    public AuthMappings()
    {
        CreateMap<Email, EmailSchema>();
        CreateMap<Phone, PhoneSchema>();
        CreateMap<FileData, FileSchema>();

        CreateMap<User, UserPublicSchema>()
            .ForMember(dest => dest.Id, s => s.MapFrom(src => src.Id.Value));

        CreateMap<User, UserPrivateSchema>()
            .ForMember(dest => dest.Id, s => s.MapFrom(src => src.Id.Value));

        CreateMap<RegisterResult, UserPrivateSchema>()
            .IncludeMembers(u => u.user);

    }
}
