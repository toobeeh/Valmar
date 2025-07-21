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

    public async Task<JwtSecurityToken> CreateJwt(int typoId, string applicationName, DateTime expiry,
        List<string> scopes, string redirectUri, int? verifiedAppId = null)
    {
        logger.LogTrace(
            "CreateJwt(typoId: {TypoId}, applicationName: {ApplicationName}, expiry: {Expiry}, scopes: {Scopes})",
            typoId, applicationName, expiry, scopes);

        // verify all scopes exist
        var matchingScopes = await db.JwtScopes
            .Where(scope => scopes.Contains(scope.Name))
            .ToListAsync();

        if (matchingScopes.Count != scopes.Count)
        {
            throw new ArgumentException("One or more requested scopes do not exist.");
        }

        // verify member exists and is not banned
        var member = await membersDomainService.GetMemberByLogin(typoId);
        if (member.MappedFlags.Contains(MemberFlagDdo.PermaBan))
        {
            throw new InvalidOperationException("Member is banned and cannot request scopes.");
        }

        // get credentials
        var credentials = signatureService.SigningCredentials;

        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, typoId.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, member.Username),
            new Claim("redirect_uri", redirectUri),
            .. scopes.Select(scope => new Claim("scope", scope)).ToList(),
        ];

        if (verifiedAppId is { } verifiedAppIdVal)
        {
            claims.Add(new Claim("verified_app_id", verifiedAppIdVal.ToString()));
        }

        // create the JWT
        var config = options.Value;
        var token = new JwtSecurityToken(
            issuer: config.JwtIssuer,
            audience: applicationName,
            claims: claims,
            expires: expiry,
            signingCredentials: credentials
        );

        return token;
    }

    public async Task<JwtSecurityToken> CreateJwtForVerifiedApplication(
        int typoId, int applicationId)
    {
        logger.LogTrace(
            "CreateJwtForVerifiedApplication(typoId: {TypoId}, applicationId: {ApplicationId})",
            typoId, applicationId);

        // get application from db
        var app = await db.JwtVerifiedApplications.FirstOrDefaultAsync(app => app.Id == applicationId);
        if (app == null)
        {
            throw new ArgumentException("Application not found.");
        }

        return await CreateJwt(
            typoId,
            app.Name,
            DateTime.UtcNow.AddMilliseconds(app.JwtExpiry),
            app.JwtScopes.Split(",").ToList(),
            app.RedirectUri,
            app.Id
        );
    }

    public string GetJwtString(JwtSecurityToken jwt)
    {
        logger.LogTrace("GetJwtString(jwt: {Jwt})", jwt);

        // return the JWT as a string
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(jwt);
    }
}