using AutoMapper;
using Google.Protobuf.WellKnownTypes;

namespace tobeh.Valmar.Server.Mappers;

public class BasicMapperProfile : Profile
{
    public BasicMapperProfile()
    {
        CreateMap<DateTimeOffset, Timestamp>()
            .ConstructUsing(time => Timestamp.FromDateTimeOffset(time));
        CreateMap<DateTimeOffset, long>()
            .ConstructUsing(time => time.ToUnixTimeMilliseconds());
    }
}