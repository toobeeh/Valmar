using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Util.NChunkTree.Bubbles;

namespace tobeh.Valmar.Server.Mappers;

public class SplitMapperProfile : Profile
{
    public SplitMapperProfile()
    {
        CreateMap<SplitDefinitionDdo, SplitReply>();
        CreateMap<SplitRewardDdo, SplitRewardReply>();
        CreateMap<AvailableSplitsDdo, AvailableSplitsReply>();
        CreateMap<DropboostDdo, ActiveDropboostReply>();
    }
}