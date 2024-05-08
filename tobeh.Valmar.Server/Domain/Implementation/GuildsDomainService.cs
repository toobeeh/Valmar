using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Exceptions;

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

        return await ConvertToDdo(guild);
    }

    public async Task<GuildDetailDdo> GetGuildByDiscordId(long discordId)
    {
        var guild = await db.LobbyBotOptions.FirstOrDefaultAsync(
            guild => guild.GuildId == discordId);

        if (guild is null)
        {
            throw new EntityNotFoundException($"Guild with discord id {discordId} does not exist");
        }

        return await ConvertToDdo(guild);
    }

    public async Task<LobbyBotOptionEntity> UpdateGuildOptions(long guildId, string name, string prefix,
        long? channelId = null)
    {
        logger.LogTrace("UpdateGuildOptions(guildId={guildId}, name={name})", guildId, name);

        var options = await db.LobbyBotOptions.FirstOrDefaultAsync(entity => entity.GuildId == guildId);
        if (options is null)
        {
            throw new EntityNotFoundException($"No guild options for id {guildId}");
        }

        options.Name = name;
        options.Prefix = prefix;
        options.ChannelId = channelId;

        db.LobbyBotOptions.Update(options);
        await db.SaveChangesAsync();

        return options;
    }

    public async Task<LobbyBotOptionEntity> GetGuildOptionsByGuildId(long guildId)
    {
        logger.LogTrace("GetGuildOptionsByGuildId(guildId={guildId})", guildId);

        var options = await db.LobbyBotOptions.FirstOrDefaultAsync(entity => entity.GuildId == guildId);
        if (options is null)
        {
            throw new EntityNotFoundException($"No guild options for id {guildId}");
        }

        return options;
    }

    private async Task<GuildDetailDdo> ConvertToDdo(LobbyBotOptionEntity guild)
    {
        logger.LogTrace("ConvertToDdo(guild={guild})", guild);

        var memberCount =
            await db.ServerConnections.CountAsync(connection => connection.GuildId == guild.GuildId);

        var details = new GuildDetailDdo(
            guild.GuildId,
            guild.Invite,
            guild.Name,
            memberCount
        );

        return details;
    }
}