using tobeh.Valmar.Server.Database;
using tobeh.Valmar.Server.Domain.Classes;

namespace tobeh.Valmar.Server.Domain;

public interface IDropsDomainService
{
    Task ScheduleDrop(int delaySeconds, int? eventDropId);
    Task<Tuple<int, int>> CalculateDropDelayBounds(int playerCount, double boostFactor);
    Task<double> GetCurrentDropBoost();
    Task<ClaimDropResultDdo> ClaimDrop(long dropId, bool leagueMode);
    Task RewardDrop(int login, int? eventDropId, double value);
    Task LogDropClaim(long dropId, long discordId, long claimTimestamp, string lobbyKey, int catchMs, int? eventDropId);
    Task<CurrrentDropEntity> GetScheduledDrop();
}