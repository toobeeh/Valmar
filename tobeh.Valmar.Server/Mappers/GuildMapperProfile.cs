using AutoMapper;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Mappers;

public class GuildMapperProfile : Profile
{
    public GuildMapperProfile()
    {
        CreateMap<GuildDetailDdo, GuildReply>();
    }
}