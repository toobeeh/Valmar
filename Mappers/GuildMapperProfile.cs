using AutoMapper;
using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Mappers;

public class GuildMapperProfile : Profile
{
    public GuildMapperProfile()
    {
        CreateMap<GuildDetailDto, GuildReply>();
    }
}