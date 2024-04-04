using AutoMapper;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Util.NChunkTree.Bubbles;

namespace Valmar.Mappers;

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