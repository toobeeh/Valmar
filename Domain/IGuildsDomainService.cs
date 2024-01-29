using Valmar.Database;
using Valmar.Domain.Classes;

namespace Valmar.Domain;

public interface IGuildsDomainService
{
    Task<GuildDetailDto> GetGuildByObserveToken(int observeToken);
}