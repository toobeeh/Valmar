using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Valmar.Database;
using Valmar.Util;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation.Drops;

public class PersistentDropChunk(PalantirContext db) : IDropChunk
{
    private long? _dropIndexStart, _dropIndexEnd;
    public long? DropIndexStart => _dropIndexStart;
    public long? DropIndexEnd => _dropIndexEnd;
    public DateTimeOffset? DropTimestampStart { get; private set; }
    public DateTimeOffset? DropTimestampEnd { get; private set; }
    public void SetChunkSize(long? start, long? end)
    {
        _dropIndexStart = start;
        _dropIndexEnd = end;

        if (start != null)
        {
            DropTimestampStart = DropHelper.ParseDropTimestamp(
                db.PastDrops.First(d => d.DropId == start).ValidFrom);
        }

        if (end != null)
        {
            DropTimestampEnd = DropHelper.ParseDropTimestamp(
                db.PastDrops.First(d => d.DropId == end).ValidFrom);
        }
    }
    
    public async Task<double> GetTotalLeagueWeight(string id)
    {
        var drops = db.PastDrops;
        var weights = await drops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && d.CaughtLobbyPlayerId == id)
            .Select(d => d.LeagueWeight)
            .ToListAsync();
        var score = weights.Sum(d => Weight(d));
        
        return score;
    }

    public async Task<double> GetLeagueWeightInTimespan(string id, DateTimeOffset start, DateTimeOffset end)
    {
        var startStamp = DropHelper.FormatDropTimestamp(start);
        var endStamp = DropHelper.FormatDropTimestamp(end);
        
        var drops = db.PastDrops;
        var weights = await drops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && d.ValidFrom.CompareTo(startStamp) >= 0
                        && d.ValidFrom.CompareTo(endStamp) < 0
                        && d.CaughtLobbyPlayerId == id)
            .Select(d => d.LeagueWeight)
            .ToListAsync();
        var score = weights.Sum(d => Weight(d));
        
        return score;
    }
    
    public async Task<int> GetTotalLeagueCount(string id)
    {
        var count = await db.PastDrops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && d.CaughtLobbyPlayerId == id)
            .CountAsync();
        return count;
    }

    public async Task<int> GetLeagueCountInTimespan(string id, DateTimeOffset start, DateTimeOffset end)
    {
        var startStamp = DropHelper.FormatDropTimestamp(start);
        var endStamp = DropHelper.FormatDropTimestamp(end);
        
        var count = await db.PastDrops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && d.ValidFrom.CompareTo(startStamp) >= 0
                        && d.ValidFrom.CompareTo(endStamp) < 0
                        && d.CaughtLobbyPlayerId == id)
            .CountAsync();
        return count;
    }

    private double Weight(double catchMs)
    {
        if (catchMs < 0) return 0;
        if (catchMs > 1000) return 0.3;
        var weight =  -1.78641975945623 * Math.Pow(10, -9) * Math.Pow(catchMs, 4) + 0.00000457264006980028 * Math.Pow(catchMs, 3) - 0.00397188791256729 * Math.Pow(catchMs, 2) + 1.21566760222325 * catchMs;
        return weight / 100;
    }
}