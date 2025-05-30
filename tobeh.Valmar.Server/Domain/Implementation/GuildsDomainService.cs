using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Exceptions;
using tobeh.Valmar.Server.Util;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class GuildsDomainService(
    ILogger<GuildsDomainService> logger,
    PalantirContext db) : IGuildsDomainService
{
    public async Task<GuildDetailDdo> GetGuildByInvite(int invite)
    {
        var guild = await db.LobbyBotOptions.FirstOrDefaultAsync(
            guild => guild.Invite == invite);
        if (guild is null)
        {
            throw new EntityNotFoundException($"Guild with invite {invite} does not exist");
        }

        var claim = await db.LobbyBotClaims
            .FirstOrDefaultAsync(claim => claim.GuildId == guild.GuildId);
        var worker = claim is not null
            ? await db.LobbyBotInstances.FirstOrDefaultAsync(instance => instance.Id == claim.InstanceId)
            : null;

        return await ConvertToDdo(guild, worker?.BotId);
    }

    public async Task<List<int>> GetGuildSupporters(long guildId)
    {
        logger.LogTrace("GetGuildSupporters(guildId={guildId})", guildId);

        var supporters = await db.LobbyBotClaims
            .Where(claim => claim.GuildId == guildId)
            .Join(db.Members, inner => inner.Login, outer => outer.Login,
                (claim, member) => member)
            .ToListAsync();

        var activeSupporters = supporters
            .Where(supporter => FlagHelper.GetFlags(supporter.Flag)
                .Any(flag => flag is MemberFlagDdo.Patron or MemberFlagDdo.Admin))
            .Select(supporter => supporter.Login)
            .ToList();

        return activeSupporters;
    }

    public async Task BanMemberFromGuild(int login, long guildId)
    {
        logger.LogTrace("BanMemberFromGuild(login={login}, guildId={guildId})", login, guildId);

        var connection =
            await db.ServerConnections.FirstOrDefaultAsync(conn => conn.Login == login && conn.GuildId == guildId);
        if (connection is null)
        {
            db.ServerConnections.Add(new ServerConnectionEntity
            {
                Ban = true,
                GuildId = guildId,
                Login = login
            });
        }
        else
        {
            if (connection.Ban)
            {
                logger.LogWarning("Member {login} is already banned from guild {guildId}", login, guildId);
                throw new ApplicationException("Member is already banned");
            }

            connection.Ban = true;
            db.ServerConnections.Update(connection);
        }

        await db.SaveChangesAsync();
    }

    public async Task UnbanMemberFromGuild(int login, long guildId)
    {
        logger.LogTrace("UnbanMemberFromGuild(login={login}, guildId={guildId})", login, guildId);

        var connection =
            await db.ServerConnections.FirstOrDefaultAsync(conn => conn.Login == login && conn.GuildId == guildId);
        if (connection is null)
        {
            throw new EntityNotFoundException($"No connection for login {login} and guild {guildId}");
        }

        if (connection.Ban is false)
        {
            logger.LogWarning("Member {login} is not banned from guild {guildId}", login, guildId);
            throw new ApplicationException("Member is not banned");
        }

        db.ServerConnections.Remove(connection);
        await db.SaveChangesAsync();
    }

    public async Task<List<int>> GetBannedMembers(long guildId)
    {
        logger.LogTrace("GetBannedMembers(guildId={guildId})", guildId);

        var bannedMembers = await db.ServerConnections
            .Where(conn => conn.GuildId == guildId && conn.Ban)
            .Select(conn => conn.Login)
            .ToListAsync();

        return bannedMembers;
    }

    public async Task<GuildDetailDdo> GetGuildByDiscordId(long discordId)
    {
        logger.LogTrace("GetGuildByDiscordId(discordId={discordId})", discordId);

        var guild = await db.LobbyBotOptions.FirstOrDefaultAsync(
            guild => guild.GuildId == discordId);

        if (guild is null)
        {
            throw new EntityNotFoundException($"Guild with discord id {discordId} does not exist");
        }

        var claim = await db.LobbyBotClaims
            .FirstOrDefaultAsync(claim => claim.GuildId == guild.GuildId);
        var worker = claim is not null
            ? await db.LobbyBotInstances.FirstOrDefaultAsync(instance => instance.Id == claim.InstanceId)
            : null;

        return await ConvertToDdo(guild, worker?.BotId);
    }

    public async Task<LobbyBotOptionEntity> UpdateGuildOptions(long guildId, string name, string prefix,
        bool showInvite, bool proxyLinks,
        long? channelId = null, string? botName = null)
    {
        logger.LogTrace("UpdateGuildOptions(guildId={guildId}, name={name})", guildId, name);

        var supporters = await GetGuildSupporters(guildId);
        if (supporters.Count == 0)
        {
            throw new UserOperationException(
                "Guild is not supported by any patrons and is therefore no typo home server.",
                false);
        }

        var options = await db.LobbyBotOptions.FirstOrDefaultAsync(entity => entity.GuildId == guildId);
        if (options is null)
        {
            throw new EntityNotFoundException($"No guild options for id {guildId}");
        }

        if (botName?.Length is > 20 or 0)
        {
            throw new UserOperationException("Bot name must be either default or between 1-20 characters.");
        }

        options.Name = name;
        options.Prefix = prefix;
        options.ChannelId = channelId;
        options.BotName = botName;
        options.ShowInvite = showInvite;
        options.ProxyLinks = proxyLinks;

        db.LobbyBotOptions.Update(options);
        await db.SaveChangesAsync();

        return options;
    }

    public async Task<LobbyBotOptionEntity> GetGuildOptionsByGuildId(long guildId)
    {
        logger.LogTrace("GetGuildOptionsByGuildId(guildId={guildId})", guildId);

        var supporters = await GetGuildSupporters(guildId);
        if (supporters.Count == 0)
        {
            throw new UserOperationException(
                "Guild is not supported by any patrons and is therefore no typo home server.",
                false);
        }

        var options = await db.LobbyBotOptions.FirstOrDefaultAsync(entity => entity.GuildId == guildId);
        if (options is null)
        {
            throw new EntityNotFoundException($"No guild options for id {guildId}");
        }

        return options;
    }

    public async Task<List<ServerWebhookEntity>> GetGuildWebhooks(long guildId)
    {
        logger.LogTrace("GetGuildWebhooks(guildId={guildId})", guildId);

        var webhooks = await db.ServerWebhooks
            .Where(webhook => webhook.GuildId == guildId)
            .ToListAsync();

        return webhooks;
    }

    public async Task RemoveGuildWebhook(long guildId, string name)
    {
        logger.LogTrace("RemoveGuildWebhook(guildId={guildId}, name={name})", guildId, name);

        var supporters = await GetGuildSupporters(guildId);
        if (supporters.Count == 0)
        {
            throw new UserOperationException(
                "Guild is not supported by any patrons and is therefore no typo home server.",
                false);
        }

        var webhook = await db.ServerWebhooks
            .FirstOrDefaultAsync(webhook => webhook.GuildId == guildId && webhook.Name == name);

        if (webhook is null)
        {
            throw new EntityNotFoundException($"No webhook with name {name} for guild {guildId}");
        }

        db.ServerWebhooks.Remove(webhook);
        await db.SaveChangesAsync();
    }

    public async Task<ServerWebhookEntity> AddGuildWebhook(long guildId, string url, string name)
    {
        logger.LogTrace("AddGuildWebhook(guildId={guildId}, url={url}, name={name})", guildId, url, name);

        var supporters = await GetGuildSupporters(guildId);
        if (supporters.Count == 0)
        {
            throw new UserOperationException(
                "Guild is not supported by any patrons and is therefore no typo home server.",
                false);
        }

        var guildWebhooks = await GetGuildWebhooks(guildId);
        if (guildWebhooks.Any(webhook => webhook.Name == name))
        {
            throw new UserOperationException($"Guild {guildId} already has a webhook with name {name}");
        }

        var webhook = new ServerWebhookEntity
        {
            GuildId = guildId,
            Url = url,
            Name = name
        };

        await db.ServerWebhooks.AddAsync(webhook);
        await db.SaveChangesAsync();

        return webhook;
    }

    private async Task<GuildDetailDdo> ConvertToDdo(LobbyBotOptionEntity guild, long? botId)
    {
        logger.LogTrace("ConvertToDdo(guild={guild})", guild);

        var memberCount =
            await db.ServerConnections.CountAsync(connection => connection.GuildId == guild.GuildId);

        var onlineMemberCount = await db.ServerConnections
            .Where(connection => connection.GuildId == guild.GuildId)
            .Join(db.SkribblOnlinePlayers,
                connection => connection.Login,
                player => player.Login,
                (connection, player) => player)
            .CountAsync();

        var supporters = await GetGuildSupporters(guild.GuildId);

        var details = new GuildDetailDdo(
            guild.GuildId,
            guild.Invite,
            guild.Name,
            memberCount,
            onlineMemberCount,
            supporters,
            botId
        );

        return details;
    }
}