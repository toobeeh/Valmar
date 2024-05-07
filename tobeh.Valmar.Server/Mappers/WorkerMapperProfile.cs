using AutoMapper;
using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Mappers;

public class WorkerMapperProfile : Profile
{
    public WorkerMapperProfile()
    {
        CreateMap<LobbyBotInstanceEntity, InstanceDetailsMessage>();
        CreateMap<LobbyBotOptionEntity, GuildOptionsMessage>();
    }
}
