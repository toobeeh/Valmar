using AutoMapper;
using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Mappers;

public class AuthorizationMapperProfile : Profile
{
    public AuthorizationMapperProfile()
    {
        CreateMap<Oauth2ScopeEntity, ScopeMessage>();
        CreateMap<Oauth2ClientEntity, OAuth2ClientMessage>()
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OwnerTypoId, opt => opt.MapFrom(src => src.Owner))
            .ForMember(dest => dest.Scopes, opt => opt.MapFrom(src => SplitList(src.Scopes)))
            .ForMember(dest => dest.RedirectUris, opt => opt.MapFrom(src => SplitList(src.RedirectUris)));
    }

    private static List<string> SplitList(string csvList)
    {
        return csvList
            .Split(',')
            .Select(scope => scope.Trim())
            .Where(scope => !string.IsNullOrEmpty(scope))
            .ToList();
    }
}