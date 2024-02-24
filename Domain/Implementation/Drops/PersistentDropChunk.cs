using System.Collections.Concurrent;
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
    
    public async Task<double> GetLeagueWeight(string id)
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

    public async Task<double> GetLeagueWeight(string id, DateTimeOffset? start, DateTimeOffset? end)
    {
        var startStamp = start is { } startDate ? DropHelper.FormatDropTimestamp(startDate) : null;
        var endStamp = end is { } endDate ? DropHelper.FormatDropTimestamp(endDate) : null;
        
        var drops = db.PastDrops;
        var weights = await drops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && (startStamp == null || d.ValidFrom.CompareTo(startStamp) >= 0)
                        && (endStamp == null ||  d.ValidFrom.CompareTo(endStamp) < 0)
                        && d.CaughtLobbyPlayerId == id)
            .Select(d => d.LeagueWeight)
            .ToListAsync();
        var score = weights.Sum(d => Weight(d));
        
        return score;
    }
    
    public async Task<int> GetLeagueCount(string id)
    {
        return await GetLeagueCount(id, null, null);
    }

    public async Task<int> GetLeagueCount(string id, DateTimeOffset? start, DateTimeOffset? end)
    {
        var startStamp = start is { } startDate ? DropHelper.FormatDropTimestamp(startDate) : null;
        var endStamp = end is { } endDate ? DropHelper.FormatDropTimestamp(endDate) : null;
        
        var count = await db.PastDrops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && (startStamp == null || d.ValidFrom.CompareTo(startStamp) >= 0)
                        && (endStamp == null ||  d.ValidFrom.CompareTo(endStamp) < 0)
                        && d.CaughtLobbyPlayerId == id)
            .CountAsync();
        return count;
    }
    
    public async Task<double> GetLeagueAverageTime(string id)
    {
        return await GetLeagueAverageTime(id, null, null);
    }

    public async Task<double> GetLeagueAverageTime(string id, DateTimeOffset? start, DateTimeOffset? end)
    {
        var startStamp = start is { } startDate ? DropHelper.FormatDropTimestamp(startDate) : null;
        var endStamp = end is { } endDate ? DropHelper.FormatDropTimestamp(endDate) : null;
        
        var time = await db.PastDrops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && (startStamp == null || d.ValidFrom.CompareTo(startStamp) >= 0)
                        && (endStamp == null ||  d.ValidFrom.CompareTo(endStamp) < 0)
                        && d.CaughtLobbyPlayerId == id)
            .Select(d => d.LeagueWeight)
            .AverageAsync(t => (int?)t);
        return time ?? 0;
    }
    
    public async Task<StreakResult> GetLeagueStreak(string id)
    {
        return await GetLeagueStreak(id, null, null);
    }
    
    public async Task<StreakResult> GetLeagueStreak(string id, DateTimeOffset? start, DateTimeOffset? end)
    {
        var startStamp = start is { } startDate ? DropHelper.FormatDropTimestamp(startDate) : null;
        var endStamp = end is { } endDate ? DropHelper.FormatDropTimestamp(endDate) : null;
        
        var streak = await db.PastDrops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && (startStamp == null || d.ValidFrom.CompareTo(startStamp) >= 0)
                        && (endStamp == null ||  d.ValidFrom.CompareTo(endStamp) < 0))
            .OrderBy(d => d.ValidFrom)
            .GroupBy(d => d.DropId)
            .Select(group => new { DropId = group.Key, Caught = group.Any(d => d.CaughtLobbyPlayerId == id) })
            .ToListAsync();
        
        var streakResult = new StreakResult(-1, -1, -1);
        var currentStreak = 0;
        var longestStreak = 0;
        foreach (var drop in streak)
        {
            if (drop.Caught)
            {
                currentStreak++;
                if(currentStreak > longestStreak) longestStreak = currentStreak;
            }
            else
            {
                // update tail at streak loss if not set yet
                if(streakResult.Tail == -1) streakResult = streakResult with { Tail = currentStreak };
                currentStreak = 0;
            }
        }
        
        // check if tail was not set
        if(streakResult.Tail == -1) streakResult = streakResult with { Tail = currentStreak };
        
        // set max and head of streak
        streakResult = streakResult with { Head = currentStreak, Streak = longestStreak };
        
        return streakResult;
    }

    public async Task<EventResult> GetEventLeagueDetails(int eventId, string userid, int userLogin)
    {
        // get eventdrops for faster processing
        var drops = await db.EventDrops
            .Where(evd => evd.EventId == eventId)
            .Select(evd => evd.EventDropId)
            .ToArrayAsync();
        
        // get drops weights of eventdrops, either redeemable or already redeemed
        var leagueEventdropWeights = await db.PastDrops
            .Where(d => d.LeagueWeight > 0
                        && drops.Any(id => id == Math.Abs(d.EventDropId))
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && d.CaughtLobbyPlayerId == userid)
            .Select(drop => new {EventDropId = Math.Abs(drop.EventDropId), Weight = drop.LeagueWeight, Redeemable = drop.EventDropId > 0 })
            .ToListAsync();

        // credits that are still redeemable (drops that can be converted to the credit table)
        var redeemableCredits = leagueEventdropWeights
            .Where(d => d.Redeemable)
            .GroupBy(d => d.EventDropId)
            .Select(g => new {EventDropId = g.Key, Credit = g.Sum(w => Weight(w.Weight))});
                
        // amount of credits that already has been converted to the credits table
        var redeemedCredits = leagueEventdropWeights.Where(d => !d.Redeemable).Sum(w => Weight(w.Weight));

        // amount of regular drops that contributed to credit -> needed for loss rate
        var regularCaughtSum = await db.PastDrops
            .Where(d => d.LeagueWeight == 0
                        && drops.Any(id => id == Math.Abs(d.EventDropId))
                        && (DropIndexStart == null || d.DropId >= DropIndexStart)
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd)
                        && d.CaughtLobbyPlayerId == userid)
            .CountAsync();
        
        // build maps
        var redeemableCreditMap = new ConcurrentDictionary<int, double>();
        double progress = regularCaughtSum + redeemedCredits;
        
        foreach (var redeemableCredit in redeemableCredits)
        {
            redeemableCreditMap[redeemableCredit.EventDropId] = redeemableCredit.Credit;
            progress += redeemableCredit.Credit;
        }

        return new EventResult(redeemableCreditMap, progress);
    }

    private double Weight(double catchMs)
    {
        if (catchMs < 0) return 0;
        if (catchMs > 1000) return 0.3;
        var weight =  -1.78641975945623 * Math.Pow(10, -9) * Math.Pow(catchMs, 4) + 0.00000457264006980028 * Math.Pow(catchMs, 3) - 0.00397188791256729 * Math.Pow(catchMs, 2) + 1.21566760222325 * catchMs;
        return weight / 100;
    }
}