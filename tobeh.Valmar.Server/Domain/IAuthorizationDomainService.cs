using System.IdentityModel.Tokens.Jwt;
using tobeh.Valmar.Server.Database;

namespace tobeh.Valmar.Server.Domain;

public interface IAuthorizationDomainService
{
    Task<List<JwtScopeEntity>> GetAvailableScopes();
    string GetJwtString(JwtSecurityToken jwt);
    Task<JwtSecurityToken> CreateJwt(int typoId, string applicationName, DateTime expiry, List<string> scopes);
}