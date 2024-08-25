using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface IGuildsDomainService
{
    Task<GuildDetailDdo> GetGuildByInvite(int invite);
    Task<GuildDetailDdo> GetGuildByDiscordId(long discordId);

    Task<LobbyBotOptionEntity> UpdateGuildOptions(long guildId, string name, string prefix,
        long? channelId = null, string? botName = null);

    Task<LobbyBotOptionEntity> GetGuildOptionsByGuildId(long guildId);
    Task<List<ServerWebhookEntity>> GetGuildWebhooks(long guildId);
    Task RemoveGuildWebhook(long guildId, string name);
    Task<ServerWebhookEntity> AddGuildWebhook(long guildId, string url, string name);
    Task<List<int>> GetGuildSupporters(long guildId);
}