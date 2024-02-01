using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Classes.JSON;
using Valmar.Domain.Exceptions;

namespace Valmar.Domain.Implementation;

public class MembersDomainService(
    ILogger<MembersDomainService> logger, 
    IGuildsDomainService guildsService,
    PalantirContext db) : IMembersDomainService
{

    public async Task<MemberDdo> GetMemberByLogin(int login)
    {
        logger.LogTrace("GetMemberByLogin(login={login})", login);

        var member = await db.Members.FirstOrDefaultAsync(member => member.Login == login);
        if (member is null)
        {
            throw new EntityNotFoundException($"No member found for login {login}");
        }
        
        // parse details from json
        var memberDetails = ValmarJsonParser.TryParse<MemberJson>(member.Member1, logger);

        // build ddo
        return new MemberDdo(
            member.Bubbles,
            member.Drops,
            member.Sprites,
            member.Scenes ?? "",
            member.Flag,
            member.RainbowSprites ?? "",
            Convert.ToInt64(memberDetails.UserId),
            memberDetails.UserName,
            Convert.ToInt32(memberDetails.UserLogin),
            memberDetails.Guilds.Select(guild => Convert.ToInt32(guild.ObserveToken)).ToList()
        );
    }

    public async Task<MemberDdo> GetMemberByDiscordId(long id)
    {
        logger.LogTrace("GetMemberByDiscordId(id={id})", id);
        
        // find likely candidates to avoid parsing a large amount of data
        var candidates = await db.Members
            .Where(member => member.Member1.Contains(id.ToString()))
            .Select(member => member.Member1)
            .ToListAsync();
        var match = candidates
            .ConvertAll(candidate => ValmarJsonParser.TryParse<MemberJson>(candidate, logger))
            .FirstOrDefault(candidate => candidate.UserId == id.ToString());

        if (match is null)
        {
            throw new EntityNotFoundException($"No member found for id {id}");
        }

        return await GetMemberByLogin(Convert.ToInt32(match.UserLogin)); // accept performance loss for code reusability
    }

    public async Task<List<MemberSearchDdo>> SearchMember(string query)
    {
        logger.LogTrace("SearchMember(query={query})", query);

        var matches = await db.Members
            .Where(member => member.Member1.Contains(query) || member.Bubbles.ToString() == query)
            .Select(member => member.Member1)
            .ToListAsync();
            
        return matches.ConvertAll(member =>
            {
                var parsed = ValmarJsonParser.TryParse<MemberJson>(member, logger);
                return new MemberSearchDdo(parsed.UserName, Convert.ToInt32(parsed.UserLogin), JsonSerializer.Serialize(member));
            });
    }

    public async Task<string> GetRawMemberByLogin(int login)
    {
        logger.LogTrace("GetRawMemberByLogin(login={login})", login);

        var member = await db.Members.FirstOrDefaultAsync(member => member.Login == login);
        if (member is null)
        {
            throw new EntityNotFoundException($"No member found for login {login}");
        }

        return JsonSerializer.Serialize(member);
    }

    public async Task<string> GetAccessTokenByLogin(int login)
    {
        logger.LogTrace("GetAccessTokenByLogin(login={login})", login);

        var member = await db.Members.FirstOrDefaultAsync(member => member.Login == login);
        if (member is null)
        {
            throw new EntityNotFoundException($"No member found for login {login}");
        }
        
        var token = await db.AccessTokens.FirstOrDefaultAsync(token => token.Login == login);
        if (token is null)
        {
            token = new AccessTokenEntity() { AccessToken1 = GenerateAccessToken(), Login = login };
            db.AccessTokens.Add(token);
            await db.SaveChangesAsync();
        }

        return token.AccessToken1;
    }

    public async Task UpdateMemberDiscordId(int login, long newId)
    {
        logger.LogTrace("UpdateMemberDiscordId(login={login}, newId={newId})", login, newId);

        var member = await db.Members.FirstOrDefaultAsync(member => member.Login == login);
        if (member is null)
        {
            throw new EntityNotFoundException($"No member found for login {login}");
        }

        var parsedMember = ValmarJsonParser.TryParse<MemberJson>(member.Member1, logger);
        if (parsedMember.UserId == newId.ToString())
        {
            throw new EntityConflictException($"Member with login {login} is already linked with the discord id {newId}");
        }
        
        // check if there is already a member for the new discord id and transfer its bubbles and drops, and remove old bubble traces
        try
        {
            var existingNewMember = await GetMemberByDiscordId(newId); // use this function for efficient querying by discord id through json column
            
            logger.LogTrace("Found account for new discord id, data will be transferred: {existingNewMember}", existingNewMember);
            member.Bubbles += existingNewMember.Bubbles;
            member.Drops += existingNewMember.Drops;

            // start tracking deprecated entities as deleted
            db.BubbleTraces.RemoveRange(db.BubbleTraces.Where(trace => trace.Login == existingNewMember.Login));
            db.Members.RemoveRange(db.Members.Where(m => m.Login == existingNewMember.Login));
        }
        catch(Exception e) {
            logger.LogTrace("No account for new discord id found, transferring no data");
        }

        // update member JSON
        var newParsedMember = parsedMember with { UserId = newId.ToString() };
        member.Member1 = JsonSerializer.Serialize(newParsedMember);
        
        // delete drops with old user id due to PK reasons
        var userDrops = await db.PastDrops.Where(drop => drop.CaughtLobbyPlayerId == parsedMember.UserId).ToListAsync();
        db.PastDrops.RemoveRange(userDrops);
        
        // execute updates
        db.Members.Update(member);
        await db.SaveChangesAsync();
        
        // add updated drops again
        logger.LogTrace("Transferring {drops} to new discord id {id}", userDrops.Count, newId);
        foreach (var drop in userDrops) drop.CaughtLobbyPlayerId = newParsedMember.UserId;
        db.PastDrops.AddRange(userDrops);
        await db.SaveChangesAsync();
    }

    public async Task ClearMemberDropboost(int login)
    {
        logger.LogTrace("ClearMemberDropboost(login={login})", login);

        var boost = await db.DropBoosts.FirstOrDefaultAsync(boost => boost.Login == login);

        if (boost is not null)
        {
            db.DropBoosts.Remove(boost);
            await db.SaveChangesAsync();
        }
    }

    public async Task ConnectToServer(int login, int serverToken)
    {
        logger.LogTrace("ConnectToServer(login={login}, serverToken={serverToken})", login, serverToken);

        var member = await db.Members.FirstOrDefaultAsync(member => member.Login == login);
        if (member is null)
        {
            throw new EntityNotFoundException($"No member found for login {login}");
        }

        var parsedMember = ValmarJsonParser.TryParse<MemberJson>(member.Member1, logger);
        if (parsedMember.Guilds.Any(guild => guild.ObserveToken == serverToken.ToString()))
        {
            throw new EntityAlreadyExistsException($"Member {login} is already connected to {serverToken}");
        }
        
        var guild = await guildsService.GetGuildByObserveToken(serverToken);

        var newGuilds = parsedMember.Guilds.Append(new GuildPropertiesJson(
            guild.GuildId.ToString(),
            guild.ChannelId.ToString(),
            guild.MessageId.ToString(),
            guild.ObserveToken.ToString(),
            guild.Name))
            .ToArray();
        var newMember = parsedMember with { Guilds = newGuilds };
        var newMemberString = JsonSerializer.Serialize(newMember);
        member.Member1 = newMemberString;

        db.Update(member);
        await db.SaveChangesAsync();
    }

    public async Task DisconnectFromServer(int login, int serverToken)
    {
        logger.LogTrace("DisconnectFromServer(login={login}, serverToken={serverToken})", login, serverToken);

        var member = await db.Members.FirstOrDefaultAsync(member => member.Login == login);
        if (member is null)
        {
            throw new EntityNotFoundException($"No member found for login {login}");
        }

        var parsedMember = ValmarJsonParser.TryParse<MemberJson>(member.Member1, logger);
        var newGuilds = parsedMember.Guilds.Where(guild => guild.ObserveToken != serverToken.ToString())
            .ToArray();
        var newMember = parsedMember with { Guilds = newGuilds };
        var newMemberString = JsonSerializer.Serialize(newMember);
        member.Member1 = newMemberString;

        db.Update(member);
        await db.SaveChangesAsync();
    }

    private string GenerateAccessToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(50);
        var base64 = Convert.ToBase64String(bytes);
        var result = Regex.Replace(base64,"[^A-Za-z0-9]","");
        return result;
    }
}