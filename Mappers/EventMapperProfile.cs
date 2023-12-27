using AutoMapper;
using Valmar.Database;

namespace Valmar.Mappers;

public class EventMapperProfile : Profile
{
    public EventMapperProfile()
    {
        CreateMap<EventEntity, EventReply>();
        CreateMap<EventDropEntity, EventDropReply>();
    }
}