using AutoMapper;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Mappers;

public class SceneMapperProfile : Profile
{
    public SceneMapperProfile()
    {
        CreateMap<SceneThemeEntity, SceneThemeReply>();
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