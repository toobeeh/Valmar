using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface IGuildsDomainService
{
    Task<GuildDetailDdo> GetGuildByObserveToken(int observeToken);
    Task<GuildDetailDdo> GetGuildByDiscordId(long discordId);
}