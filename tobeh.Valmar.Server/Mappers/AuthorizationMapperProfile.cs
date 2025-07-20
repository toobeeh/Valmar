using AutoMapper;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Mappers;

public class AuthorizationMapperProfile : Profile
{
    public AuthorizationMapperProfile()
    {
        CreateMap<JwtScopeEntity, ScopeMessage>().ReverseMap();
        CreateMap<SignScopeRequestMessage, ScopeRequestDdo>().ReverseMap();
    }
}