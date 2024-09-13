using AutoMapper;
using MessengerAPI.Application.Schemas.Notifications;
using MessengerAPI.Domain.ChannelAggregate.Entities;
using MessengerAPI.Domain.ChannelAggregate.Events;

namespace MessengerAPI.Application.Common.Mappings;

public class NotificationMapping : Profile
{
    public NotificationMapping()
    {
        CreateMap<NewMessageCreated, NewMessageNotificationSchema>()
            .ForMember(dest => dest.Data, s => s.MapFrom(src => src.NewMessage));
    }
}
