using AutoMapper;
using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Mappers;

public class EventMapperProfile : Profile
{
    public EventMapperProfile()
    {
        CreateMap<EventDdo, EventReply>();
        
        CreateMap<EventDropEntity, EventDropReply>()
            .ForMember(
                drop => drop.Id, 
                opt=> opt.MapFrom(src => src.EventDropId));
    }
}