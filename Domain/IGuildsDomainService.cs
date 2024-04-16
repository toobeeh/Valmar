using Valmar.Domain.Classes;

namespace Valmar.Domain;

public interface IGuildsDomainService
{
    Task<GuildDetailDdo> GetGuildByObserveToken(int observeToken);
    Task<GuildDetailDdo> GetGuildByDiscordId(long discordId);
}