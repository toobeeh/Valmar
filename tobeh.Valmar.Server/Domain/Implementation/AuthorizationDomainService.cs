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
    public async Task<List<Oauth2ScopeEntity>> GetAvailableScopes()
    {
        logger.LogTrace("GetAvailableScopes()");

        return await db.Oauth2Scopes.ToListAsync();
    }

    public async Task<List<Oauth2ClientEntity>> GetOauth2Clients()
    {
        logger.LogTrace("GetOauth2Clients()");

        return await db.Oauth2Clients.ToListAsync();
    }

    public async Task<Oauth2ClientEntity> CreateOauth2Client(
        string name, List<string> redirectUris, List<string> scopes, int ownerTypoId)
    {
        logger.LogTrace(
            "CreateOauth2Client(name: {Name}, redirectUris: {redirectUris}, scopes: {Scopes}, ownerTypoId: {OwnerTypoId})",
            name, redirectUris, scopes, ownerTypoId);

        // verify all scopes exist
        var matchingScopes = await db.Oauth2Scopes
            .Where(scope => scopes.Contains(scope.Name))
            .ToListAsync();

        if (matchingScopes.Count != scopes.Count)
        {
            throw new ArgumentException("One or more requested scopes do not exist.");
        }

        // verify member exists and is not banned
        var member = await membersDomainService.GetMemberByLogin(ownerTypoId);
        if (member.MappedFlags.Contains(MemberFlagDdo.PermaBan))
        {
            throw new InvalidOperationException("Member is banned and cannot create clients.");
        }

        // verify member has not more than 5 clients
        var existingClients = await db.Oauth2Clients
            .Where(client => client.Owner == ownerTypoId)
            .CountAsync();

        if (existingClients >= 5)
        {
            throw new InvalidOperationException("Member has reached the maximum number of OAuth2 clients (5).");
        }

        // create the client
        var client = new Oauth2ClientEntity
        {
            Name = name,
            RedirectUris = string.Join(",", redirectUris),
            Scopes = string.Join(",", matchingScopes.Select(s => s.Name)),
            Owner = ownerTypoId,
            TokenExpiry = 60 * 60 * 24 * 365, // 1 year
            Verified = false
        };

        db.Oauth2Clients.Add(client);
        await db.SaveChangesAsync();

        return client;
    }

    public async Task<Oauth2ClientEntity> GetOauth2ClientById(int clientId)
    {
        logger.LogTrace("GetOauth2ClientById(clientId: {ClientId})", clientId);

        var client = await db.Oauth2Clients
            .FirstOrDefaultAsync(c => c.Id == clientId);

        if (client == null)
        {
            throw new ArgumentException("Client not found.");
        }

        return client;
    }

    public async Task<OAuth2AuthorizationCodeDdo> CreateOAuth2AuthorizationCode(int typoId, int clientId)
    {
        logger.LogTrace("CreateOAuth2AuthorizationCode(typoId: {TypoId}, clientId: {ClientId})", typoId, clientId);

        // verify member exists and is not banned
        var member = await membersDomainService.GetMemberByLogin(typoId);
        if (member.MappedFlags.Contains(MemberFlagDdo.PermaBan))
        {
            throw new InvalidOperationException("Member is banned and cannot request authorization codes.");
        }

        // verify client exists
        var client = await GetOauth2ClientById(clientId);

        // create the authorization code
        var code = new Oauth2AuthorizationCodeEntity
        {
            TypoId = typoId,
            ClientId = clientId,
            Code = signatureService.GenerateRandomString(30),
            Expiry = DateTimeOffset.UtcNow.AddMinutes(5).ToUnixTimeSeconds()
        };

        // remove any existing authorization codes for this member and client
        var existingCodes = await db.Oauth2AuthorizationCodes
            .Where(ac => ac.TypoId == typoId && ac.ClientId == clientId)
            .ToListAsync();

        if (existingCodes.Count != 0)
        {
            db.Oauth2AuthorizationCodes.RemoveRange(existingCodes);
        }

        db.Oauth2AuthorizationCodes.Add(code);
        await db.SaveChangesAsync();

        return new OAuth2AuthorizationCodeDdo(code.Code, code.ClientId, client.RedirectUris.Split(",").ToList());
    }

    public async Task<string> CreateOauth2Token(int typoId, int clientId, string issuer,
        string requestedAudience)
    {
        logger.LogTrace(
            "CreateOauth2Token(typoId: {typoId}, clientId: {ClientId}, issuer: {Issuer}, requestedAudience: {RequestedAudience})",
            typoId, clientId, issuer, requestedAudience);

        // get client
        var client = await GetOauth2ClientById(clientId);

        // get member
        var member = await membersDomainService.GetMemberByLogin(typoId);

        // verify issuer not null or empty
        if (string.IsNullOrWhiteSpace(issuer))
        {
            throw new ArgumentException("Issuer must be provided.");
        }

        // create access token
        var credentials = signatureService.SigningCredentials;

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, member.Login.ToString()),
            new(JwtRegisteredClaimNames.Name, member.Username),
            new(JwtRegisteredClaimNames.Azp, client.Id.ToString()),
            new("aud_name", client.Name),
            .. client.Scopes.Split(",").Select(scope => new Claim("scope", scope)).ToList(),
        ];

        if (client.Verified)
        {
            claims.Add(new Claim("client_verified", "true"));
        }

        // create the JWT
        var jwt = new JwtSecurityToken(
            issuer,
            audience: requestedAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(client.TokenExpiry),
            signingCredentials: credentials
        );

        return GetJwtString(jwt);
    }

    public async Task<string> ExchangeOauth2AuthorizationCode(string code, int clientId, string issuer)
    {
        logger.LogTrace(
            "ExchangeOauth2AuthorizationCode(code: {Code}, clientId: {ClientId}, issuer: {Issuer})",
            code, clientId, issuer);

        // get client
        var client = await GetOauth2ClientById(clientId);

        // find the authorization code
        var authCode = await db.Oauth2AuthorizationCodes
            .FirstOrDefaultAsync(ac => ac.Code == code && ac.ClientId == clientId);

        if (authCode == null)
        {
            throw new ArgumentException("Authorization code not found or invalid.");
        }

        // verify auth code client matches
        if (authCode.ClientId != clientId)
        {
            throw new ArgumentException("Authorization code does not belong to the specified client.");
        }

        // verify authorization code is not expired (5 min)
        if (authCode.Expiry < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
        {
            db.Oauth2AuthorizationCodes.Remove(authCode);
            await db.SaveChangesAsync();

            throw new InvalidOperationException("Authorization code has expired.");
        }

        var jwt = await CreateOauth2Token(authCode.TypoId, client.Id, issuer, client.Audience);

        // remove the authorization code
        db.Oauth2AuthorizationCodes.Remove(authCode);
        await db.SaveChangesAsync();

        return jwt;
    }

    public string GetJwtString(JwtSecurityToken jwt)
    {
        logger.LogTrace("GetJwtString(jwt: {Jwt})", jwt);

        // return the JWT as a string
        var handler = new JwtSecurityTokenHandler();
        return handler.WriteToken(jwt);
    }
}