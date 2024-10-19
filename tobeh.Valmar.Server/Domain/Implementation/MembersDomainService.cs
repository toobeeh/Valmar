using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Classes.JSON;
using tobeh.Valmar.Server.Domain.Exceptions;
using tobeh.Valmar.Server.Util;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class MembersDomainService(
    ILogger<MembersDomainService> logger,
    IGuildsDomainService guildsService,
    PalantirContext db) : IMembersDomainService
{
    public async Task<MemberDdo> CreateMember(long discordId, string username, bool connectTypo)
    {
        logger.LogTrace("CreateMember(discordId={discordId}, username={username}, connectTypo={connectTypo})",
            discordId, username, connectTypo);

        var memberExists = false;
        try
        {
            await GetMemberByDiscordId(discordId);
            memberExists = true;
        }
        catch
        {
            // ignore, user does not exist
        }

        if (memberExists)
        {
            throw new UserOperationException(
                $"Member can not be created, already exists with given discord ID {discordId}");
        }

        int login;
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
            Flag = 0,
            Streamcode = ""
        };

        var memberDetails = new MemberJson(
            discordId.ToString(),
            username,
            member.Login.ToString()
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

        return await ConvertToDdo(member);
    }

    public async Task<List<MemberDdo>> GetGuildMembers(long guildId)
    {
        logger.LogTrace("GetGuildMembers(guildId={guildId})", guildId);

        var guild = await guildsService.GetGuildByDiscordId(guildId);

        var members = await db.Members.Where(member =>
                db.ServerConnections.Any(connection =>
                    !connection.Ban && connection.Login == member.Login && connection.GuildId == guild.GuildId))
            .ToListAsync();

        var memberDdos = new List<MemberDdo>();
        foreach (var entity in members)
        {
            memberDdos.Add(await ConvertToDdo(entity)); // TODO review performance impact on big batches
        }

        return memberDdos;
    }

    public async Task<List<MemberDdo>> GetAllMembers()
    {
        logger.LogTrace("GetAllMembers()");

        var members = await db.Members.ToListAsync();
        var memberDdos = new List<MemberDdo>();
        foreach (var entity in members)
        {
            memberDdos.Add(await ConvertToDdo(entity)); // TODO review performance impact on big batches
        }

        return memberDdos;
    }

    public async Task<List<MemberDdo>> GetMembersByLogin(List<int> logins)
    {
        logger.LogTrace("GetMembersByLogin(logins={logins})", logins);

        var members = await db.Members.Where(member => logins.Contains(member.Login)).ToListAsync();

        var memberDdos = new List<MemberDdo>();
        foreach (var entity in members)
        {
            memberDdos.Add(await ConvertToDdo(entity)); // TODO review performance impact on big batches
        }

        return memberDdos;
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
            return new MemberSearchDdo(parsed.UserName, Convert.ToInt32(parsed.UserLogin),
                JsonSerializer.Serialize(member));
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
            throw new EntityConflictException(
                $"Member with login {login} is already linked with the discord id {newId}");
        }

        // check if there is already a member for the new discord id and transfer its bubbles and drops, and remove old bubble traces
        try
        {
            var existingNewMember =
                await GetMemberByDiscordId(
                    newId); // use this function for efficient querying by discord id through json column

            logger.LogTrace("Found account for new discord id, data will be transferred: {existingNewMember}",
                existingNewMember);
            member.Bubbles += existingNewMember.Bubbles;
            member.Drops += existingNewMember.Drops;

            // start tracking deprecated entities as deleted
            db.BubbleTraces.RemoveRange(db.BubbleTraces.Where(trace => trace.Login == existingNewMember.Login));
            db.Members.RemoveRange(db.Members.Where(m => m.Login == existingNewMember.Login));

            // update own awards of temp member
            var ownAwards = await db.Awardees.Where(awardee => awardee.OwnerLogin == existingNewMember.Login)
                .ToListAsync();
            ownAwards.ForEach(award => award.OwnerLogin = member.Login);
            db.Awardees.UpdateRange(ownAwards);

            // update received awards of temp member
            var receivedAwards = await db.Awardees.Where(awardee => awardee.AwardeeLogin == existingNewMember.Login)
                .ToListAsync();
            receivedAwards.ForEach(award => award.AwardeeLogin = member.Login);
            db.Awardees.UpdateRange(receivedAwards);
        }
        catch (Exception)
        {
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

        var guild = await guildsService.GetGuildByInvite(serverToken);
        var options = await guildsService.GetGuildOptionsByGuildId(guild.GuildId);

        if (!(options.ShowInvite ?? true))
        {
            throw new ApplicationException("Guild has not enabled connecting via invite");
        }

        var existingConnection =
            await db.ServerConnections.FirstOrDefaultAsync(entity =>
                entity.Login == login && entity.GuildId == guild.GuildId);
        if (existingConnection is not null)
        {
            if (existingConnection.Ban)
                throw new UserOperationException("User has been banned and cannot modify the server connection");
            throw new EntityAlreadyExistsException($"Member {login} is already connected to {guild.Name}");
        }

        var connectionEntity = new ServerConnectionEntity
        {
            Ban = false,
            GuildId = guild.GuildId,
            Login = login
        };
        db.ServerConnections.Add(connectionEntity);
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

        var guild = await guildsService.GetGuildByInvite(serverToken);
        var connection =
            await db.ServerConnections.FirstOrDefaultAsync(entity =>
                entity.Login == login && entity.GuildId == guild.GuildId);

        if (connection is null)
        {
            throw new EntityNotFoundException($"Member {login} is not connected to guild {guild.Invite}");
        }

        if (connection.Ban)
        {
            throw new UserOperationException("User has been banned and cannot modify the server connection");
        }

        db.ServerConnections.Remove(connection);
        await db.SaveChangesAsync();
    }

    private string GenerateAccessToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(50);
        var base64 = Convert.ToBase64String(bytes);
        var result = Regex.Replace(base64, "[^A-Za-z0-9]", "");
        return result;
    }

    private async Task<MemberDdo> ConvertToDdo(MemberEntity member)
    {
        logger.LogTrace("ConvertToDdo(member={member})", member);

        // parse details from json
        var memberDetails = ValmarJsonParser.TryParse<MemberJson>(member.Member1, logger);

        // parse next award pack open date
        var mappedFlags = FlagHelper.GetFlags(member.Flag);
        if (mappedFlags.Contains(MemberFlagDdo.Admin) && !mappedFlags.Contains(MemberFlagDdo.Patron))
        {
            mappedFlags.Add(MemberFlagDdo.Patron);
        }

        // parse patronized details
        var patronize =
            InventoryHelper.ParsePatronizedMember(
                mappedFlags.Any(f => f is MemberFlagDdo.Patronizer or MemberFlagDdo.Admin)
                    ? member.Patronize
                    : null);

        var awardPackCooldown = mappedFlags.Any(flag => flag is MemberFlagDdo.Admin or MemberFlagDdo.Patron) ? 5 : 7;
        var nextAwardPack = member.AwardPackOpened is { } timestamp
            ? DateTimeOffset.FromUnixTimeMilliseconds(timestamp).AddDays(awardPackCooldown)
            : DateTimeOffset.MinValue;

        var connections = await db.ServerConnections
            .Where(connection => connection.Login == member.Login && !connection.Ban)
            .Join(db.LobbyBotOptions, conn => conn.GuildId, opt => opt.GuildId, (conn, opt) => opt.Invite)
            .ToListAsync();

        var serverHomeClaim = await db.LobbyBotClaims.FirstOrDefaultAsync(claim => claim.Login == member.Login);
        var serverHomeNextDate = serverHomeClaim is { } claim
            ? DateTimeOffset.FromUnixTimeMilliseconds(claim.ClaimTimestamp).AddDays(7)
            : DateTimeOffset.MinValue;

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
            connections,
            patronize.Item1,
            mappedFlags.Contains(MemberFlagDdo.Patron) ? member.Emoji : null,
            mappedFlags,
            nextAwardPack,
            patronize.Item2.AddDays(7),
            serverHomeNextDate
        );
    }
}