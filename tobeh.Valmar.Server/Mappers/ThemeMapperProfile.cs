using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Mappers;

public class ThemeMapperProfile : Profile
{
    public ThemeMapperProfile()
    {
        CreateMap<string, ThemeShareReply>().ConvertUsing(s => new() {Id = s});
        CreateMap<string, ThemeDataReply>().ConvertUsing(s => new() {ThemeJson = s});
        CreateMap<ThemeListingDdo, ThemeListingReply>();
    }
}