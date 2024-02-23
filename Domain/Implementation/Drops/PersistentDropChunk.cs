using Microsoft.EntityFrameworkCore;
using Valmar.Database;

namespace Valmar.Domain.Implementation.Drops;

public class PersistentDropChunk(PalantirContext db) : IDropChunk
{
    private long? _dropIndexStart, _dropIndexEnd;
    public long? DropIndexStart => _dropIndexStart;
    public long? DropIndexEnd => _dropIndexEnd;
    public void SetChunkSize(long? start, long? end)
    {
        _dropIndexStart = start;
        _dropIndexEnd = end;
    }
    
    public async Task<double> GetTotalLeagueWeight(string id)
    {
        var drops = db.PastDrops;
        var weights = await drops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId <= DropIndexEnd) 
                        && d.CaughtLobbyPlayerId == id)
            .Select(d => d.LeagueWeight)
            .ToListAsync();
        var score = weights.Sum(d => Weight(d));
        
        return score;
    }

    public async Task<double> GetLeagueWeightInTimespan(string id, DateOnly start, DateOnly end)
    {
        /*var ctx = new PalantirContext();
        var drops = ctx.PastDrops;*/
        /*var score = (await drops
                .Where(d => (DropIndexStart == null || d.DropId >= DropIndexStart) 
                            && (DropIndexEnd == null || d.DropId <= DropIndexEnd) 
                            && d.CaughtLobbyPlayerId == id)
                .Select(d => d.LeagueWeight)
                .ToListAsync())
            .Sum(d => d == 0 ? 1 : Weight(d));*/
        
        /*ctx.Dispose();*/
        return 1; //score;
    }

    private double Weight(double catchMs)
    {
        if (catchMs < 0) return 0;
        if (catchMs > 1000) return 30;
        var weight =  -1.78641975945623 * Math.Pow(10, -9) * Math.Pow(catchMs, 4) + 0.00000457264006980028 * Math.Pow(catchMs, 3) - 0.00397188791256729 * Math.Pow(catchMs, 2) + 1.21566760222325 * catchMs;
        return weight / 100;
    }
}