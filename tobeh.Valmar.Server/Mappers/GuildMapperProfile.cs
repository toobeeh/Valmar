using AutoMapper;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Mappers;

public class GuildMapperProfile : Profile
{
    public GuildMapperProfile()
    {
        CreateMap<GuildDetailDdo, GuildReply>();
        CreateMap<ServerWebhookEntity, GuildWebhookMessage>();
    }
}