using AutoMapper;
using MessengerAPI.Application.Auth.Commands.Register;
using MessengerAPI.Domain.Channel;
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
        
        CreateMap<FileData, FileSchema>()
            .ForMember(dest => dest.OwnerId, s => s.MapFrom(src => src.OwnerId.Value))
            .ForMember(dest => dest.Sha256, s => s.MapFrom(src => Convert.ToHexString(src.Sha256).ToLower()))
            .ForMember(dest => dest.DisplaySize, s => s.MapFrom(src => GetHumanReadableFileSize(src.Size)));
        
        CreateMap<Channel, ChannelSchema>()
            .ForMember(dest => dest.Id, s => s.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.OwnerId, s => s.MapFrom(src => src.OwnerId.Value));

        CreateMap<RegisterResult, UserPrivateSchema>()
            .IncludeMembers(u => u.user);

    }

    private string GetHumanReadableFileSize(long sizeInBytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB", "TB" };
        double len = sizeInBytes;
        int order = 0;
        
        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }
        
        return $"{len:0.##} {sizes[order]}";
    }
}
