using System.IdentityModel.Tokens.Jwt;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface IAuthorizationDomainService
{
    Task<List<JwtScopeEntity>> GetAvailableScopes();
    Task<string> SignScopeRequest(ScopeRequestDdo request);
    Task<JwtSecurityToken> CreateJwt(ScopeRequestDdo request, string requestSignature);
    string GetJwtString(JwtSecurityToken jwt);
}