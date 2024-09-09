using AutoMapper;
using MessengerAPI.Application.Auth.Commands.Register;
using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.ChannelAggregate.ValueObjects;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.ValueObjects;
using MessengerAPI.Presentation.Schemas.Common;

namespace MessengerAPI.Presentation.Common.Mapping;

public class AuthMappings : Profile
{
    public AuthMappings()
    {
        CreateMap<ChannelId, Guid>().ConvertUsing(src => src.Value);
        CreateMap<UserId, Guid>().ConvertUsing(src => src.Value);
        CreateMap<MessageId, int>().ConvertUsing(src => src.Value);


        CreateMap<Email, EmailSchema>();
        CreateMap<Phone, PhoneSchema>();
        CreateMap<FileData, FileSchema>();
        CreateMap<Reaction, ReactionSchema>();
        CreateMap<Message, MessageSchema>();
        CreateMap<User, UserPublicSchema>();
        CreateMap<User, UserPrivateSchema>();
        
        CreateMap<FileData, FileSchema>()
            .ForMember(dest => dest.Sha256, s => s.MapFrom(src => Convert.ToHexString(src.Sha256).ToLower()))
            .ForMember(dest => dest.DisplaySize, s => s.MapFrom(src => GetHumanReadableFileSize(src.Size)));
        
        CreateMap<Channel, ChannelSchema>();

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
