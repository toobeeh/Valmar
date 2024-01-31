using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Domain;

public interface IGuildsDomainService
{
    Task<GuildDetailDdo> GetGuildByObserveToken(int observeToken);
}