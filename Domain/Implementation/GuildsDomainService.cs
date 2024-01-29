using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Classes;
using Valmar.Domain.Classes.JSON;
using Valmar.Domain.Exceptions;

namespace Valmar.Domain.Implementation;

public class GuildsDomainService(
    ILogger<GuildsDomainService> logger, 
    PalantirContext db) : IGuildsDomainService
{
    public async Task<GuildDetailDto> GetGuildByObserveToken(int observeToken)
    {
        var guild = await db.Palantiris.FirstOrDefaultAsync(guild => guild.Token == observeToken.ToString());
        if (guild is null)
        {
            throw new EntityNotFoundException($"Guild with token {observeToken} does not exist");
        }

        var guildProperties = ParseGuildProperties(guild.Palantir);
        var memberCount =
            await db.Members.Where(member => member.Member1.Contains(observeToken.ToString())).CountAsync();

        var details = new GuildDetailDto(
            Convert.ToInt64(guildProperties.GuildID),
            Convert.ToInt64(guildProperties.ChannelID),
            Convert.ToInt64(guildProperties.MessageID),
            observeToken,
            guildProperties.GuildName,
            memberCount
        );

        return details;
    }

    private GuildProperties ParseGuildProperties(string guildJson)
    {
        GuildProperties? guildProperties = null;
        try
        {
            guildProperties = JsonSerializer.Deserialize<GuildProperties>(guildJson, ValmarJsonOptions.JsonSerializerOptions);
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
}