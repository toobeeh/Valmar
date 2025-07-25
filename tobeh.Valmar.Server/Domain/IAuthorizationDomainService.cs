using System.IdentityModel.Tokens.Jwt;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface IAuthorizationDomainService
{
    Task<List<Oauth2ScopeEntity>> GetAvailableScopes();
    string GetJwtString(JwtSecurityToken jwt);

    Task<List<Oauth2ClientEntity>> GetOauth2Clients();

    Task<Oauth2ClientEntity> CreateOauth2Client(
        string name, string redirectUri, List<string> scopes, int ownerTypoId);

    Task<Oauth2ClientEntity> GetOauth2ClientById(int clientId);

    Task<OAuth2AuthorizationCodeDdo> CreateOAuth2AuthorizationCode(int typoId, int clientId);

    Task<string> ExchangeOauth2AuthorizationCode(string code, int clientId, string issuer);
    Task<string> CreateOauth2Token(int typoId, int clientId, string issuer, string requestedAudience);
}