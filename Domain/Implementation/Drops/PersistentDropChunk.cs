using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Valmar.Database;
using Valmar.Util;
using Valmar.Util.NChunkTree;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation.Drops;

/// <summary>
/// Implementation which extends the dropchunkleaf node by the chunk specific functions
/// Main feature is the access and calculation of drop-related data from persistance
/// </summary>
/// <param name="services"></param>
/// <param name="provider"></param>
/// <param name="context"></param>
public class PersistentDropChunk : DropChunkLeaf, IDropChunk
{
    private readonly PalantirContext _db; // guarantee new db instance
    
    public override IDropChunk Chunk => this;

    private long? _dropIndexStart, _dropIndexEnd;

    public PersistentDropChunk(IServiceProvider services, DropChunkTreeProvider provider, NChunkTreeNodeContext context) : base(services, provider, context)
    {
        _db = ActivatorUtilities.CreateInstance<PalantirContext>(services);
        
        // init chunk with saved range
        Provider.PersistentChunkContext.TryGetValue(NodeId, out var range);
        if (range != null)
        {
            _dropIndexStart = range.Start;
            _dropIndexEnd = range.End;
            DropTimestampStart = range.StartDate;
            DropTimestampEnd = range.EndDate;
        }
    }

    public long? DropIndexStart => _dropIndexStart;
    public long? DropIndexEnd => _dropIndexEnd;
    public DateTimeOffset? DropTimestampStart { get; private set; }
    public DateTimeOffset? DropTimestampEnd { get; private set; }
    
    /// <summary>
    /// Sets the range of the chunk;
    /// Fetches also the dates of the range bounds and stores to the provider for re-fetching
    /// Other instances that are currently alive will be outdated 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void SetChunkSize(long? start, long? end)
    {
        _dropIndexStart = start;
        _dropIndexEnd = end;

        if (start != null)
        {
            DropTimestampStart = DropHelper.ParseDropTimestamp(
                _db.PastDrops.First(d => d.DropId == start).ValidFrom);
        }

        if (end != null)
        {
            DropTimestampEnd = DropHelper.ParseDropTimestamp(
                _db.PastDrops.First(d => d.DropId == end).ValidFrom);
        }

        Provider.PersistentChunkContext[NodeId] = new PersistentDropChunkRange(start, end, DropTimestampStart, DropTimestampEnd);
    }
    
    public async Task<List<long>> EvaluateSubChunks(int chunkSize)
    {
        // find indexes to index chunks
        var drops = _db.PastDrops
            .Where(d => d.LeagueWeight > 0
                 && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                 && (DropIndexEnd == null || d.DropId < DropIndexEnd))
            .OrderBy(d => d.DropId)
            .AsEnumerable()
            .Where((drop, index) => (index) % chunkSize == 0)
            .Select(item => Convert.ToInt64(item.DropId))
            .ToList();

        return drops;
    }
    
    public async Task<double> GetLeagueWeight(string id)
    {
        var drops = _db.PastDrops;
        var weights = await drops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && d.CaughtLobbyPlayerId == id)
            .Select(d => d.LeagueWeight)
            .ToListAsync();
        var score = weights.Sum(d => DropHelper.Weight(d));
        
        return score;
    }

    public async Task<double> GetLeagueWeight(string id, DateTimeOffset? start, DateTimeOffset? end)
    {
        var startStamp = start is { } startDate ? DropHelper.FormatDropTimestamp(startDate) : null;
        var endStamp = end is { } endDate ? DropHelper.FormatDropTimestamp(endDate) : null;
        
        var drops = _db.PastDrops;
        var weights = await drops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && (startStamp == null || d.ValidFrom.CompareTo(startStamp) >= 0)
                        && (endStamp == null ||  d.ValidFrom.CompareTo(endStamp) < 0)
                        && d.CaughtLobbyPlayerId == id)
            .Select(d => d.LeagueWeight)
            .ToListAsync();
        var score = weights.Sum(d => DropHelper.Weight(d));
        
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
        
        var count = await _db.PastDrops
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
        
        var time = await _db.PastDrops
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
    
    public async Task<IList<string>> GetLeagueParticipants()
    {
        return await GetLeagueParticipants(null, null);
    }

    public async Task<IList<string>> GetLeagueParticipants(DateTimeOffset? start, DateTimeOffset? end)
    {
        var startStamp = start is { } startDate ? DropHelper.FormatDropTimestamp(startDate) : null;
        var endStamp = end is { } endDate ? DropHelper.FormatDropTimestamp(endDate) : null;
        
        var participants = await _db.PastDrops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && (startStamp == null || d.ValidFrom.CompareTo(startStamp) >= 0)
                        && (endStamp == null ||  d.ValidFrom.CompareTo(endStamp) < 0))
            .Select(d => d.CaughtLobbyPlayerId)
            .Distinct()
            .ToListAsync();
        return participants;
    }
    
    public async Task<StreakResult> GetLeagueStreak(string id)
    {
        return await GetLeagueStreak(id, null, null);
    }
    
    public async Task<StreakResult> GetLeagueStreak(string id, DateTimeOffset? start, DateTimeOffset? end)
    {
        var startStamp = start is { } startDate ? DropHelper.FormatDropTimestamp(startDate) : null;
        var endStamp = end is { } endDate ? DropHelper.FormatDropTimestamp(endDate) : null;
        
        var streak = await _db.PastDrops
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
        var drops = await _db.EventDrops
            .Where(evd => evd.EventId == eventId)
            .Select(evd => evd.EventDropId)
            .ToArrayAsync();
        
        // get drops weights of eventdrops, either redeemable or already redeemed
        var leagueEventdropWeights = await _db.PastDrops
            .Where(d => d.LeagueWeight > 0
                        && drops.Any(id => id == Math.Abs(d.EventDropId))
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && d.CaughtLobbyPlayerId == userid)
            .Select(drop => new {EventDropId = Math.Abs(drop.EventDropId), Weight = drop.LeagueWeight, Redeemable = drop.EventDropId > 0, DropId = drop.DropId })
            .ToListAsync();

        // credits that are still redeemable (drops that can be converted to the credit table)
        var redeemableCredits = leagueEventdropWeights
            .Where(d => d.Redeemable)
            .GroupBy(d => d.EventDropId)
            .Select(g => new {EventDropId = g.Key, Credit = g.Select(w => new { Weight = DropHelper.Weight(w.Weight), w.DropId })});
                
        // amount of credits that already has been converted to the credits table
        var redeemedCredits = leagueEventdropWeights.Where(d => !d.Redeemable).Sum(w => DropHelper.Weight(w.Weight));

        // amount of regular drops that contributed to credit -> needed for loss rate
        var regularCaughtSum = await _db.PastDrops
            .Where(d => d.LeagueWeight == 0
                        && drops.Any(id => id == Math.Abs(d.EventDropId))
                        && (DropIndexStart == null || d.DropId >= DropIndexStart)
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd)
                        && d.CaughtLobbyPlayerId == userid)
            .CountAsync();
        
        // build maps
        var redeemableCreditMap = new ConcurrentDictionary<int, ConcurrentDictionary<long, double>>();
        double progress = regularCaughtSum + redeemedCredits;
        
        foreach (var redeemableCredit in redeemableCredits)
        {
            var dict = redeemableCredit.Credit.ToDictionary(credit => credit.DropId, credit => credit.Weight);
            
            redeemableCreditMap[redeemableCredit.EventDropId] = new ConcurrentDictionary<long, double>(dict);
            progress += redeemableCredit.Credit.Sum(c => c.Weight);
        }

        return new EventResult(redeemableCreditMap, progress);
    }
    
    public async Task<Dictionary<string, LeagueResult>> GetLeagueResults(DateTimeOffset? start, DateTimeOffset? end)
    {
        var startStamp = start is { } startDate ? DropHelper.FormatDropTimestamp(startDate) : null;
        var endStamp = end is { } endDate ? DropHelper.FormatDropTimestamp(endDate) : null;
        
        // get participants for easier processing
        var participants = (await GetLeagueParticipants(start, end)).ToArray();
        
        // get data from db
        var drops = await _db.PastDrops
            .Where(d => d.LeagueWeight > 0
                        && (DropIndexStart == null || d.DropId >= DropIndexStart) 
                        && (DropIndexEnd == null || d.DropId < DropIndexEnd) 
                        && (startStamp == null || d.ValidFrom.CompareTo(startStamp) >= 0)
                        && (endStamp == null ||  d.ValidFrom.CompareTo(endStamp) < 0))
            .OrderBy(d => d.ValidFrom)
            .GroupBy(d => d.DropId)
            .Select(group => new { DropId = group.Key, Caught = group.Select(drop => new { Id = drop.CaughtLobbyPlayerId, Time = drop.LeagueWeight }) })
            .ToListAsync();
        
        // process data
        var streakTails = new Dictionary<string, int>();
        var streakMaxes = new Dictionary<string, int>();
        var streakHeads = new Dictionary<string, int>(); 
        var scores = new Dictionary<string, double>();
        var counts = new Dictionary<string, int>();
        var times = new Dictionary<string, double>();

        for (var i = 0; i < drops.Count; i++)
        {
            var drop = drops[i];
            var continuesHeads = new Dictionary<string, int>();
            
            foreach (var claim in drop.Caught)
            {
                
                // continues a streak
                var thisStreak = 1;
                if (streakHeads.TryGetValue(claim.Id, out var streak))
                {
                    thisStreak = streak + 1;
                    continuesHeads[claim.Id] = thisStreak;
                }

                // starts a new streak 
                else
                {
                    continuesHeads[claim.Id] = 1;
                }
                    
                // if streak == i, tail streak is still in progress
                if (thisStreak == i + 1)
                {
                    streakTails[claim.Id] = thisStreak;
                }
                
                // update max streak 
                if (streakMaxes.TryGetValue(claim.Id, out var max))
                {
                    if (thisStreak > max) streakMaxes[claim.Id] = thisStreak;
                }
                else streakMaxes[claim.Id] = thisStreak;
                
                // add to score
                if (scores.TryGetValue(claim.Id, out var score))
                {
                    scores[claim.Id] = score + DropHelper.Weight(claim.Time);
                }
                else scores[claim.Id] = DropHelper.Weight(claim.Time);
                
                // add to count
                if (counts.TryGetValue(claim.Id, out var count))
                {
                    counts[claim.Id] = count + 1;
                }
                else counts[claim.Id] = 1;
                
                // add to times
                if (times.TryGetValue(claim.Id, out var time))
                {
                    times[claim.Id] = time + claim.Time;
                }
                else times[claim.Id] = claim.Time;
            }
            

            // reset to contain only active streaks
            streakHeads = continuesHeads;
        }
            
        // map to records from dictionaries
        var resultList = participants.Select(participant =>
        {
            counts.TryGetValue(participant, out var totalCount);
            scores.TryGetValue(participant, out var totalScore);
            scores.TryGetValue(participant, out var time);
            double averageTime = time / totalCount;
            double averageWeight = totalScore / totalCount;

            streakTails.TryGetValue(participant, out var tail);
            streakMaxes.TryGetValue(participant, out var head);
            streakHeads.TryGetValue(participant, out var max);

            var streak = new StreakResult(tail, head, max);

            return new LeagueResult(participant, totalScore, totalCount, averageTime, averageWeight, streak);
        });

        var results = resultList.ToDictionary(item => item.Id);
        return results;
    }
}