using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Mappers;

public class EventMapperProfile : Profile
{
    public EventMapperProfile()
    {
        CreateMap<EventDdo, EventReply>();
        CreateMap<EventDropDdo, EventDropReply>();
    }
}