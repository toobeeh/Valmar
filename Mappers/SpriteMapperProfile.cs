using AutoMapper;
using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Mappers;

public class SpriteMapperProfile : Profile
{
    public SpriteMapperProfile()
    {
        CreateMap<SpriteRankingDdo, SpriteRankingReply>();
        
        CreateMap<SpriteDdo, SpriteReply>()
            .ForMember(s => s.IsRainbow, 
                opt => opt.MapFrom(src => src.Rainbow == 1))
            .ForMember(s => s.IsSpecial, 
                opt => opt.MapFrom(src => src.Special))
            .ForMember(s => s.EventDropId, 
                opt => opt.MapFrom(src => MapEventDropId(src)))
            .ForMember(s => s.Artist, 
                opt => opt.MapFrom(src => MapArtist(src)));
    }

    private static int? MapEventDropId(SpriteDdo sprite)
    {
        if (sprite.EventDropId == 0) return null;
        return sprite.EventDropId;
    }

    private static string? MapArtist(SpriteDdo sprite)
    {
        if (string.IsNullOrWhiteSpace(sprite.Artist)) return null;
        return sprite.Artist;
    }
}