using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Valmar.Database;
using Valmar.Domain.Classes.Param;

namespace Valmar.Mappers;

public class BasicMapperProfile : Profile
{
    public BasicMapperProfile()
    {
        CreateMap<DateTimeOffset, Timestamp>()
            .ConstructUsing(time => Timestamp.FromDateTimeOffset(time));
    }
}
