using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Util.NChunkTree.Bubbles;

namespace tobeh.Valmar.Server.Mappers;

public class StatMapperProfile : Profile
{
    public StatMapperProfile()
    {
        CreateMap<BubbleTimespanRangeDdo, BubbleTimespanRangeReply>();
        CreateMap<LeaderboardRankDdo, LeaderboardRankMessage>();
        CreateMap<BubbleProgressEntryDdo, BubbleProgressEntryMessage>();
        CreateMap<LeaderboardModeDdo, LeaderboardMode>().ReverseMap();
        CreateMap<BubbleProgressIntervalModeDdo, BubbleProgressIntervalMode>().ReverseMap();
    }
}