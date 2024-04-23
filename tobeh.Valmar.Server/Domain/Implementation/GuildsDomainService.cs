using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;
using tobeh.Valmar.Server.Domain.Classes.JSON;
using tobeh.Valmar.Server.Domain.Exceptions;

namespace tobeh.Valmar.Server.Domain.Implementation;

public class GuildsDomainService(
    ILogger<GuildsDomainService> logger, 
    PalantirContext db) : IGuildsDomainService
{
    public async Task<GuildDetailDdo> GetGuildByObserveToken(int observeToken)
    {
        var guild = await db.Palantiris.FirstOrDefaultAsync(
            guild => guild.Token == observeToken.ToString("00000000") || guild.Token == observeToken.ToString()); // TODO fix inconsistency of 0 paddings in db
        if (guild is null)
        {
            throw new EntityNotFoundException($"Guild with token {observeToken} does not exist");
        }

        return await ConvertToDdo(guild);
    }
    
    public async Task<GuildDetailDdo> GetGuildByDiscordId(long discordId)
    {
        var guilds = await db.Palantiris.Where(
            guild => guild.Palantir.Contains(discordId.ToString()))
            .ToListAsync(); // filter out matching candidates

        var matchingGuild =
            (await Task.WhenAll(guilds.Select(ConvertToDdo))).FirstOrDefault(guild => guild.GuildId == discordId);
        
        if (matchingGuild is null)
        {
            throw new EntityNotFoundException($"Guild with discord id {discordId} does not exist");
        }

        return matchingGuild;
    }

    private GuildPropertiesJson ParseGuildProperties(string guildJson)
    {
        GuildPropertiesJson? guildProperties = null;
        try
        {
            guildProperties = JsonSerializer.Deserialize<GuildPropertiesJson>(guildJson, ValmarJsonOptions.JsonSerializerOptions);
        }
        catch(Exception e)
        {
            logger.LogError(e, "Failed to parse guild");
            guildProperties = null;
        }
        
        if (guildProperties is null)
        {
            throw new NullReferenceException($"Failed to parse guild properties: \n{guildJson}");
        }
        
        return guildProperties;
    }

    private async Task<GuildDetailDdo> ConvertToDdo(PalantiriEntity guild)
    {
        logger.LogTrace("ConvertToDdo(guild={guild})", guild);
        
        var guildProperties = ParseGuildProperties(guild.Palantir);
        var memberCount =
            await db.Members.Where(member => member.Member1.Contains(guild.Token)).CountAsync(); // TODO improve safety, this is only a "inaccurate" count for performance

        var details = new GuildDetailDdo(
            Convert.ToInt64(guildProperties.GuildId),
            Convert.ToInt64(guildProperties.ChannelId),
            Convert.ToInt64(guildProperties.MessageId),
            Convert.ToInt32(guild.Token),
            guildProperties.GuildName,
            memberCount
        );

        return details;
    }
}