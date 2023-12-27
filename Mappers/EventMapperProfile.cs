using AutoMapper;
using Valmar.Database;

namespace Valmar.Mappers;

public class EventMapperProfile : Profile
{
    public EventMapperProfile()
    {
        CreateMap<EventEntity, EventReply>()
            .ForMember(
                evt => evt.Id,
                opt => opt.MapFrom(src => src.EventId))
            .ForMember(
                evt => evt.Start,
                opt => opt.MapFrom(src => src.ValidFrom))
            .ForMember(
                evt => evt.Name,
                opt => opt.MapFrom(src => src.EventName))
            .ForMember(
                evt => evt.Length,
                opt => opt.MapFrom(src => src.DayLength));
        
        CreateMap<EventDropEntity, EventDropReply>()
            .ForMember(
                drop => drop.Id, 
                opt=> opt.MapFrom(src => src.EventDropId));
    }
}