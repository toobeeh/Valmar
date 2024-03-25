using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Classes.JSON;
using Valmar.Domain.Exceptions;
using Valmar.Util;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation;

public class MembersDomainService(
    ILogger<MembersDomainService> logger, 
    IGuildsDomainService guildsService,
    DropChunkTreeProvider dropTree,
    PalantirContext db) : IMembersDomainService
{
    public async Task<MemberDdo> CreateMember(long discordId, string username, bool connectTypo)
    {
        logger.LogTrace("CreateMember(discordId={discordId}, username={username}, connectTypo={connectTypo})", discordId, username, connectTypo);
        
        int login ;
        do
        {
            login = new Random().Next(99999999);
        } while (await db.Members.AnyAsync(member => member.Login == login));
        
        var member = new MemberEntity()
        {
            Bubbles = 0,
            Drops = 0,
            Sprites = "",
            Login = login,
            Flag = 0
        };

        var memberDetails = new MemberJson(
            discordId.ToString(),
            username,
            member.Login.ToString(),
            []
        );
        member.Member1 = JsonSerializer.Serialize(memberDetails);

        db.Members.Add(member);
        await db.SaveChangesAsync();

        if (connectTypo) await ConnectToServer(login, 79177353);
        return await GetMemberByLogin(login);
    }

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
        
        // get league drop stats
        //var drops = dropTree.GetTree().Chunk;
        //var value = await drops.GetLeagueWeight(memberDetails.UserId);
        //var count = await drops.GetLeagueCount(memberDetails.UserId);
        
        // parse patronized details
        long? patronizedId = member.Patronize is { } patronizeString ? Convert.ToInt64(patronizeString.Split("#")[0]) : null;

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
            memberDetails.Guilds.Select(guild => Convert.ToInt32(guild.ObserveToken)).ToList(),
            patronizedId,
            FlagHelper.HasFlag(member.Flag, MemberFlagDdo.Patron) ? member.Emoji : null,
            FlagHelper.GetFlags(member.Flag)
        );
    }
    
    public async Task<MemberDdo> GetPatronizedMemberOfPatronizer(long patronizerId)
    {
        logger.LogTrace("GetPatronizedMemberOfPatronizer(patronizerId={patronizerId})", patronizerId);
        
        var patronizer = await GetMemberByDiscordId(patronizerId);
        if (patronizer.PatronizedDiscordId is { } patronizedId)
        {
            var patronized = await GetMemberByDiscordId(patronizedId);
            return patronized;
        }
        throw new EntityNotFoundException("User has no member patronized");
    }
    
    public async Task<List<MemberJson>> GetMemberInfosFromDiscordIds(List<long> ids)
    {
        logger.LogTrace("GetMemberInfosFromDiscordIds(ids={ids})", ids);
        var idArray = ids.Select(id => id.ToString());
        
        // find likely candidates to avoid parsing a large amount of data
        var candidates = await db.Members
            .Select(member => member.Member1)
            .ToListAsync();
        var matches = candidates
            .Where(member => idArray.Any(id => member.Contains(id)))
            .Select(candidate => ValmarJsonParser.TryParse<MemberJson>(candidate, logger))
            .ToList();

        return matches;
    }

    public async Task<int> GetMemberLoginFromDiscordId(long id)
    {
        logger.LogTrace("GetMemberLoginFromDiscordId(id={id})", id);
        
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

        return Convert.ToInt32(match.UserLogin);
    }

    public async Task<MemberDdo> GetMemberByDiscordId(long id)
    {
        logger.LogTrace("GetMemberByDiscordId(id={id})", id);
        
        // get login
        var login = await GetMemberLoginFromDiscordId(id);

        return await GetMemberByLogin(login); // accept performance loss for code reusability
    }

    public async Task<MemberDdo> GetMemberByAccessToken(string token)
    {
        logger.LogTrace("GetMemberByAccessToken(token={token})", token);

        var accessToken = await db.AccessTokens.FirstOrDefaultAsync(entity => entity.AccessToken1 == token);
        if (accessToken is null)
        {
            throw new EntityNotFoundException($"No access token found found for token {token}");
        }

        return await GetMemberByLogin(accessToken.Login);
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
            
            // update own awards of temp member
            var ownAwards = await db.Awardees.Where(awardee => awardee.OwnerLogin == existingNewMember.Login).ToListAsync();
            ownAwards.ForEach(award => award.OwnerLogin = member.Login);
            db.Awardees.UpdateRange(ownAwards);
            
            // update received awards of temp member
            var receivedAwards = await db.Awardees.Where(awardee => awardee.AwardeeLogin == existingNewMember.Login).ToListAsync();
            receivedAwards.ForEach(award => award.AwardeeLogin = member.Login);
            db.Awardees.UpdateRange(receivedAwards);

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
        var newGuilds = parsedMember.Guilds.Where(guild => guild.ObserveToken != serverToken.ToString("00000000") && guild.ObserveToken != serverToken.ToString())
            .ToArray();
        
        if(newGuilds.Length == parsedMember.Guilds.Length)
        {
            throw new EntityNotFoundException($"Member {login} is not connected to guild {serverToken}");
        }
        
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