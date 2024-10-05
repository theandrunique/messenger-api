using AutoMapper;
using MessengerAPI.Application.Schemas.Common;
using MessengerAPI.Domain.ChannelAggregate;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.Common.Entities;
using MessengerAPI.Domain.UserAggregate;
using MessengerAPI.Domain.UserAggregate.ValueObjects;

namespace MessengerAPI.Application.Common.Mappings;

public class EntitiesMapping : Profile
{
    public EntitiesMapping()
    {
        CreateMap<FileData, string>().ConvertUsing(src => src.Url);
        CreateMap<FileData?, string?>().ConvertUsing(src => src != null ? src.Url : null);
        CreateMap<UserImage, string>().ConvertUsing(src => src.Key);

        CreateMap<Channel, ChannelSchema>();
        CreateMap<Message, MessageSchema>();
        CreateMap<Reaction, ReactionSchema>();
        CreateMap<User, UserPublicSchema>();
        CreateMap<User, UserPrivateSchema>();

        CreateMap<FileData, FileSchema>()
            .ForMember(dest => dest.Sha256, s => s.MapFrom(src => Convert.ToHexString(src.Sha256).ToLower()))
            .ForMember(dest => dest.DisplaySize, s => s.MapFrom(src => GetHumanReadableFileSize(src.Size)));
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
