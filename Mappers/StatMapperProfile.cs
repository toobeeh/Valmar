using AutoMapper;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Util.NChunkTree.Bubbles;

namespace Valmar.Mappers;

public class StatMapperProfile : Profile
{
    public StatMapperProfile()
    {
        CreateMap<BubbleTimespanRangeDdo, BubbleTimespanRangeReply>();
    }
}