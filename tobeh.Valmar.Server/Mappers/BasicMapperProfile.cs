using AutoMapper;
using Google.Protobuf.WellKnownTypes;

namespace tobeh.Valmar.Server.Mappers;

public class BasicMapperProfile : Profile
{
    public BasicMapperProfile()
    {
        CreateMap<DateTimeOffset, Timestamp>()
            .ConstructUsing(time => Timestamp.FromDateTimeOffset(time));
        CreateMap<Timestamp, DateTimeOffset>()
            .ConstructUsing(time => time.ToDateTimeOffset());
        CreateMap<DateTimeOffset, long>()
            .ConstructUsing(time => time.ToUnixTimeMilliseconds());
        CreateMap<long, Timestamp>()
            .ConstructUsing(ticks => Timestamp.FromDateTimeOffset(DateTimeOffset.FromUnixTimeMilliseconds(ticks)));
        CreateMap<Timestamp, long>()
            .ConstructUsing(time => time.ToDateTimeOffset().ToUnixTimeMilliseconds());
    }
}