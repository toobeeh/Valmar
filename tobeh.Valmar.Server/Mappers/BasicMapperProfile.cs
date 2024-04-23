using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes.Param;

namespace tobeh.Valmar.Server.Mappers;

public class BasicMapperProfile : Profile
{
    public BasicMapperProfile()
    {
        CreateMap<DateTimeOffset, Timestamp>()
            .ConstructUsing(time => Timestamp.FromDateTimeOffset(time));
    }
}
