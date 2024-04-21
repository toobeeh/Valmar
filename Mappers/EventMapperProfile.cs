using AutoMapper;
using Valmar.Domain.Classes;

namespace Valmar.Mappers;

public class EventMapperProfile : Profile
{
    public EventMapperProfile()
    {
        CreateMap<EventDdo, EventReply>();
        CreateMap<EventDropDdo, EventDropReply>();
    }
}