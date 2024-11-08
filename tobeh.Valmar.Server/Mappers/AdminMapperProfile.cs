using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes.Param;

namespace tobeh.Valmar.Server.Mappers;

public class AdminMapperProfile : Profile
{
    private static readonly Dictionary<OnlineItemType, string> TypeTranslations = new()
    {
        { OnlineItemType.Sprite, "sprite" },
        { OnlineItemType.Scene, "scene" },
        { OnlineItemType.ColorShift, "shift" },
        { OnlineItemType.Rewardee, "rewardee" },
        { OnlineItemType.Award, "award" },
        { OnlineItemType.SceneTheme, "sceneTheme" }
    };

    private static readonly Dictionary<string, OnlineItemType> ReverseTypeTranslations =
        TypeTranslations.ToDictionary(kv => kv.Value, kv => kv.Key);

    public AdminMapperProfile()
    {
        CreateMap<OnlineItemType, string>().ConvertUsing(item => TypeTranslations[item]);
        CreateMap<string, OnlineItemType>().ConvertUsing(item => ReverseTypeTranslations[item]);
        CreateMap<OnlineItemMessage, OnlineItemDdo>().ReverseMap();
    }
}