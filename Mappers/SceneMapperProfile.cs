using AutoMapper;
using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Mappers;

public class SceneMapperProfile : Profile
{
    public SceneMapperProfile()
    {
        CreateMap<SceneRankingDdo, SceneRankingReply>();
        CreateMap<SceneEntity, SceneReply>()
            .ForMember(s => s.EventId, 
                opt => opt.MapFrom(src => MapEventId(src)))
            .ForMember(s => s.Artist, 
                opt => opt.MapFrom(src => MapArtist(src)));
    }

    private static int? MapEventId(SceneEntity scene)
    {
        if (scene.EventId == 0) return null;
        return scene.EventId;
    }

    private static string? MapArtist(SceneEntity scene)
    {
        if (string.IsNullOrWhiteSpace(scene.Artist)) return null;
        return scene.Artist;
    }
}