using AutoMapper;
using MessengerAPI.Contracts.Common;
using MessengerAPI.Domain.Entities.ValueObjects;
using MessengerAPI.Domain.Models.Entities;
using MessengerAPI.Domain.Models.ValueObjects;

namespace MessengerAPI.Contracts;

public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<Image, string>().ConstructUsing(src => src.Key);

        CreateMap<Channel, ChannelSchema>();
        CreateMap<Message, MessageSchema>();
        // CreateMap<Reaction, ReactionSchema>();
        CreateMap<User, UserPublicSchema>();
        CreateMap<User, UserPrivateSchema>();
        CreateMap<MessageAuthorInfo, MessageAuthorInfoSchema>();
        CreateMap<ChannelMemberInfo, ChannelMemberInfoSchema>();
        CreateMap<MessageInfo, MessageInfoSchema>();

        CreateMap<Attachment, AttachmentSchema>()
            .ForMember(a => a.Url, opt => opt.MapFrom(a => a.PreSignedUrl));
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
