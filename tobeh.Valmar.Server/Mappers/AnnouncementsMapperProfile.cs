using AutoMapper;
using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Mappers;

public class AnnouncementsMapperProfile : Profile
{
    private static readonly Dictionary<AnnouncementType, string> TypeTranslations = new()
    {
        { AnnouncementType.Announcement, "announcement" },
        { AnnouncementType.Changelog, "changelog" }
    };

    private static readonly Dictionary<string, AnnouncementType> ReverseTypeTranslations =
        TypeTranslations.ToDictionary(kv => kv.Value, kv => kv.Key);

    public AnnouncementsMapperProfile()
    {
        CreateMap<AnnouncementType, string>().ConvertUsing(item => TypeTranslations[item]);
        CreateMap<string, AnnouncementType>().ConvertUsing(item => ReverseTypeTranslations[item]);
        CreateMap<AnnouncementMessage, TypoAnnouncementEntity>().ReverseMap();
    }
}