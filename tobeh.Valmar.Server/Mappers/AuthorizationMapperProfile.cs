using AutoMapper;
using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Mappers;

public class AuthorizationMapperProfile : Profile
{
    public AuthorizationMapperProfile()
    {
        CreateMap<JwtScopeEntity, ScopeMessage>().ReverseMap();
    }
}