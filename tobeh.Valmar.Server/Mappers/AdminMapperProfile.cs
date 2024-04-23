using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes.Param;
using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Mappers;

public class AdminMapperProfile : Profile
{
    private static Dictionary<OnlineItemType, string> _typeTranslations = new()
    {
        { OnlineItemType.Sprite, "sprite" },
        { OnlineItemType.Scene, "scene" },
        { OnlineItemType.ColorShift, "shift" },
        { OnlineItemType.Rewardee, "rewardee" },
        { OnlineItemType.Award, "award" }
    };
    
    public AdminMapperProfile()
    {
        CreateMap<OnlineItemType, string>().ConvertUsing(item => _typeTranslations[item]);
        CreateMap<OnlineItemMessage, OnlineItemDdo>();
    }
}
