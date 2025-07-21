using AutoMapper;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Mappers;

public class AuthorizationMapperProfile : Profile
{
    public AuthorizationMapperProfile()
    {
        CreateMap<Oauth2ScopeEntity, ScopeMessage>();
        CreateMap<OAuth2AuthorizationCodeDdo, OAuth2AuthorizationCodeMessage>();
        CreateMap<Oauth2ClientEntity, OAuth2ClientMessage>()
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OwnerTypoId, opt => opt.MapFrom(src => src.Owner))
            .ForMember(dest => dest.Scopes, opt => opt.MapFrom(src => SplitScopes(src.Scopes)));
    }

    private static List<string> SplitScopes(string scopes)
    {
        return scopes
            .Split(',')
            .Select(scope => scope.Trim())
            .Where(scope => !string.IsNullOrEmpty(scope))
            .ToList();
    }
}