using Microsoft.EntityFrameworkCore;
using Valmar.Database;
using Valmar.Domain.Exceptions;
using Valmar.Util;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation;

public class DropsDomainService(
    ILogger<DropsDomainService> logger, 
    PalantirContext db
    ) : IDropsDomainService
{
    public async Task ScheduleDrop(int delaySeconds, int? eventDropId)
    {
        logger.LogTrace("ScheduleDrop(delaySeconds={delaySeconds}, eventDropId={eventDropId})", delaySeconds, eventDropId);

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var validFrom =
            DropHelper.FormatDropTimestamp(DateTimeOffset.FromUnixTimeMilliseconds(timestamp).AddSeconds(delaySeconds));
        
        db.NextDrops.RemoveRange(db.NextDrops);
        db.NextDrops.Add(new NextDropEntity()
        {
            CaughtLobbyKey = "",
            CaughtLobbyPlayerId = "",
            DropId = timestamp,
            EventDropId = eventDropId ?? 0,
            LeagueWeight = 0,
            ValidFrom = validFrom
        });

        await db.SaveChangesAsync();
    }
    
    public Task<Tuple<int, int>> CalculateDropDelayBounds(int playerCount, double boostFactor)
    {
        logger.LogTrace("CalculateDropDelayBounds(playerCount={playerCount}, boostFactor={boostFactor})", playerCount, boostFactor);

        if (playerCount <= 0) playerCount = 1;
        int min = 600 / playerCount;
        if (min < 30) min = 30;
        min += 20; // minimum offset

        // modify by boosts
        min = Convert.ToInt32(Math.Round(min / boostFactor, 0));
            
        return Task.FromResult(new Tuple<int, int>(min, 4*min));
    }
    
    public async Task<double> GetCurrentDropBoost()
    {
        logger.LogTrace("GetCurrentDropBoost()");

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var boost = 1 + await db.DropBoosts
            .Where(b => Convert.ToInt32(b.StartUtcs) + b.DurationS > timestamp)
            .Select(b => Convert.ToDouble(b.Factor) - 1)
            .SumAsync();

        return boost;
    }
}