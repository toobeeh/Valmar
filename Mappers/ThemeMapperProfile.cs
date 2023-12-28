using AutoMapper;
using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Mappers;

public class ThemeMapperProfile : Profile
{
    public ThemeMapperProfile()
    {
        CreateMap<string, ThemeShareReply>().ConvertUsing(s => new() {Id = s});
        CreateMap<string, ThemeDataReply>().ConvertUsing(s => new() {ThemeJson = s});
        CreateMap<ThemeListingDdo, ThemeListingReply>();
    }
}