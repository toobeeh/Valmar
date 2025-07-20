using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Util.Authorization;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class AuthorizationDomainService(
    ILogger<AuthorizationDomainService> logger,
    PalantirContext db,
    IOptions<AuthorizationConfig> options,
    IMembersDomainService membersDomainService,
    SignatureService signatureService
) : IAuthorizationDomainService
{
    public async Task<List<JwtScopeEntity>> GetAvailableScopes()
    {
        logger.LogTrace("GetAvailableScopes()");

        return await db.JwtScopes.ToListAsync();
    }

    public async Task<string> SignScopeRequest(ScopeRequestDdo request)
    {
        logger.LogTrace("SignScopeRequest(request: {Request})", request);

        // verify all scopes exist
        var matchingScopes = await db.JwtScopes
            .Where(scope => request.Scopes.Contains(scope.Name))
            .ToListAsync();

        if (matchingScopes.Count != request.Scopes.Count)
        {
            throw new ArgumentException("One or more requested scopes do not exist.");
        }

        var signature = signatureService.Sign(request.ToString());
        return signature;
    }

    public async Task<JwtSecurityToken> CreateJwt(ScopeRequestDdo request, string requestSignature)
    {
        logger.LogTrace("CreateJwt(request: {Request}, requestSignature: {RequestSignature})", request,
            requestSignature);

        // verify the signature to make sure it has been signed by the server before
        if (!signatureService.Verify(request.ToString(), requestSignature))
        {
            logger.LogWarning("CreateJwt: Invalid signature for request: {Request}", request);
            throw new ArgumentException("Invalid signature for scope request.");
        }

        // verify member exists and is not banned
        var member = await membersDomainService.GetMemberByLogin(request.TypoId);
        if (member.MappedFlags.Contains(MemberFlagDdo.PermaBan))
        {
            throw new InvalidOperationException("Member is banned and cannot request scopes.");
        }

        // get credentials
        var credentials = signatureService.SigningCredentials;

        // create the JWT
        var config = options.Value;
        var token = new JwtSecurityToken(
            issuer: config.JwtIssuer,
            audience: request.ApplicationName,
            claims:
            [
                new Claim(JwtRegisteredClaimNames.Sub, request.TypoId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, member.Username),
                .. request.Scopes.Select(scope => new Claim("scope", scope)).ToList(),
            ],
            expires: request.Expiry.UtcDateTime,
            signingCredentials: credentials
        );

        return token;
    }

    public string GetJwtString(JwtSecurityToken jwt)
    {
        logger.LogTrace("GetJwtString(jwt: {Jwt})", jwt);

        // return the JWT as a string
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(jwt);
    }
}