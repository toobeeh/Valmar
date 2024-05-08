using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface IGuildsDomainService
{
    Task<GuildDetailDdo> GetGuildByInvite(int invite);
    Task<GuildDetailDdo> GetGuildByDiscordId(long discordId);

    Task<LobbyBotOptionEntity> UpdateGuildOptions(long guildId, string name, string prefix,
        long? channelId = null);

    Task<LobbyBotOptionEntity> GetGuildOptionsByGuildId(long guildId);
}