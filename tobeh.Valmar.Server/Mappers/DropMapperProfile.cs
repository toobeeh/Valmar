using AutoMapper;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Util;

namespace tobeh.Valmar.Server.Mappers;

public class DropMapperProfile : Profile
{
    public DropMapperProfile()
    {
        CreateMap<ClaimDropResultDdo, ClaimDropResultMessage>();
        CreateMap<CurrrentDropEntity, ScheduledDropMessage>();
        CreateMap<PastDropEntity, DropHistoryEntryMessage>()
            .ForMember(message => message.CatchMs, options => options.MapFrom(entity => entity.LeagueWeight))
            .ForMember(message => message.DropDate,
                options => options.MapFrom(entity => DropHelper.ParseDropTimestamp(entity.ValidFrom)));
    }
}