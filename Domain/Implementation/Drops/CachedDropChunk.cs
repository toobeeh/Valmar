using System.Collections.Concurrent;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation.Drops;

/*
 *  Drop Store should accomplish the following:
 *  - Get user total drop value (regular and event drops)
 *  - Get user league score/streak/count in time span
 *  - get user event drop value (used/unused)
 *  - consume user eventdrop value
 *
 */
public class CachedDropChunk(IEnumerable<IDropChunk> chunks) : IDropChunk
{
    private ConcurrentDictionary<string, UserStore<string, double>> _leagueDropValue = new ();
    private ConcurrentDictionary<string, UserStore<string, int>> _leagueDropCount = new ();
    private ConcurrentDictionary<string, UserStore<string, StreakResult>> _leagueStreak = new ();
    private ConcurrentDictionary<string, UserStore<string, double>> _leagueAverageTime = new ();
    private ConcurrentDictionary<string, UserStore<string, EventResult>> _eventDetails = new ();
    
    private async Task<TSum> SumAsync<TSource, TSum>(IEnumerable<TSource> source, Func<TSource, Task<TSum>> sourceMapping, Func<TSum, TSum, TSum> aggregator, TSum seed)
    {
        TSum sum = seed;
        var sumLock = new object();
        
        await Parallel.ForEachAsync(source, async (item, token) =>
        {
            try
            {
                var value = await sourceMapping.Invoke(item);
                lock (sumLock)
                {
                    sum = aggregator.Invoke(sum, value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        });

        return sum;
    }
    
    public long? DropIndexStart => !chunks.Any() ? null : chunks.First().DropIndexStart;
    public long? DropIndexEnd => !chunks.Any() ? null : chunks.Last().DropIndexStart;
    public DateTimeOffset? DropTimestampStart => !chunks.Any() ? null : chunks.First().DropTimestampStart;
    public DateTimeOffset? DropTimestampEnd => !chunks.Any() ? null : chunks.Last().DropTimestampEnd;

    public async Task<double> GetLeagueWeight(string id)
    {
        return await GetLeagueWeight(id, null, null);
    }

    public async Task<double> GetLeagueWeight(string id, DateTimeOffset? start, DateTimeOffset? end)
    {
        // sort out if this chunk is relevant
        if (start > DropTimestampEnd) return 0d;
        if (end < DropTimestampStart) return 0d;
        
        // set end or start to null if out if chunk range
        if (start < DropTimestampStart) start = null;
        if (end >= DropTimestampEnd) end = null;
        
        // get key for request identifiers; will mostly be league requests (start/end of month) or whole chunk span so not too diverse
        var key = $"{id}//{start?.UtcTicks}//{end?.UtcTicks}";
        var store = _leagueDropValue.GetOrAdd(key,key =>  new UserStore<string, double>(key, async key =>
            await SumAsync(chunks, async c => await c.GetLeagueWeight(id, start, end), (a, b) => a+b, 0d))
        );
        return await store.Retrieve();
    }
    
    public async Task<int> GetLeagueCount(string id)
    {
        return await GetLeagueCount(id, null, null);
    }

    public async Task<int> GetLeagueCount(string id, DateTimeOffset? start, DateTimeOffset? end)
    {
        // sort out if this chunk is relevant
        if (start > DropTimestampEnd) return 0;
        if (end < DropTimestampStart) return 0;
        
        // set end or start to null if out if chunk range
        if (start < DropTimestampStart) start = null;
        if (end >= DropTimestampEnd) end = null;
        
        // get key for request identifiers; will mostly be league requests (start/end of month) or whole chunk span so not too diverse
        var key = $"{id}//{start?.UtcTicks}//{end?.UtcTicks}";
        var store = _leagueDropCount.GetOrAdd(key,key =>  new UserStore<string, int>(key, async key =>
            await SumAsync(chunks, async c => await c.GetLeagueCount(id, start, end), (a, b) => a+b, 0))
        );
        return await store.Retrieve();
    }
    
    public async Task<StreakResult> GetLeagueStreak(string id)
    {
        return await GetLeagueStreak(id, null, null);
    }
    
    public async Task<StreakResult> GetLeagueStreak(string id, DateTimeOffset? start, DateTimeOffset? end)
    {
        
        // sort out if this chunk is relevant
        if (start > DropTimestampEnd) return new StreakResult(0, 0, 0);
        if (end < DropTimestampStart) return new StreakResult(0, 0, 0);
        
        // set end or start to null if out if chunk range
        if (start < DropTimestampStart) start = null;
        if (end >= DropTimestampEnd) end = null;
        
        // get key for request identifiers; will mostly be league requests (start/end of month) or whole chunk span so not too diverse
        var key = $"{id}//{start?.UtcTicks}//{end?.UtcTicks}";
        var store = _leagueStreak.GetOrAdd(key,key =>  new UserStore<string, StreakResult>(key, async key =>
            await SumAsync(chunks, async c => await c.GetLeagueStreak(id, start, end), (a, b) =>
            {
                var instercetion = a.Head + b.Tail;
                var max = Math.Max(b.Streak, Math.Max(a.Streak, instercetion));
                return new StreakResult(a.Tail, b.Head, Math.Max(a.Streak, max));
            }, new StreakResult(0, 0, 0)))
        );
        return await store.Retrieve();
    }
    
    public async Task<double> GetLeagueAverageTime(string id)
    {
        return await GetLeagueAverageTime(id, null, null);
    }
    
    public async Task<double> GetLeagueAverageTime(string id, DateTimeOffset? start, DateTimeOffset? end)
    {
        
        // sort out if this chunk is relevant
        if (start > DropTimestampEnd) return 0;
        if (end < DropTimestampStart) return 0;
        
        // set end or start to null if out if chunk range
        if (start < DropTimestampStart) start = null;
        if (end >= DropTimestampEnd) end = null;
        
        // get key for request identifiers; will mostly be league requests (start/end of month) or whole chunk span so not too diverse
        var key = $"{id}//{start?.UtcTicks}//{end?.UtcTicks}";
        double amount = await GetLeagueCount(id, start, end);
        var store = _leagueAverageTime.GetOrAdd(key,key =>  new UserStore<string, double>(key, async key =>
            await SumAsync(chunks, async c =>
            {
                var time = await c.GetLeagueAverageTime(id, start, end);
                var chunkAmount = await c.GetLeagueCount(id, start, end);
                return time * (chunkAmount / amount);
            }, (a, b) => a + b, 0d))
        );
        return await store.Retrieve();
    }
    
    public async Task<EventResult> GetEventLeagueDetails(int eventId, string userid, int userLogin)
    {
        // get key for request identifiers
        var key = $"{eventId}//{userid}//{userLogin}";
        var store = _eventDetails.GetOrAdd(key,key =>  new UserStore<string, EventResult>(key, async key =>
            await SumAsync(chunks, async c => await c.GetEventLeagueDetails(eventId, userid, userLogin),
                (a, b) =>
                {
                    foreach (var key in b.RedeemableCredit.Keys)
                    {
                        a.RedeemableCredit.AddOrUpdate(key, b.RedeemableCredit[key], (akey, value) => value + b.RedeemableCredit[akey]);
                    }
                    
                    return a with { Progress = a.Progress + b.Progress };
                }, 
                new EventResult(new ConcurrentDictionary<int, double>(), 0)))
        );
        return await store.Retrieve();
    }
}

public class UserStore<TKey, TData>(TKey key, Func<TKey, Task<TData>> retrieval)
{
    private TData _data;
    private bool _dirty = true;

    public async Task<TData> Retrieve()
    {
        if (_dirty) _data = await retrieval.Invoke(key);
        _dirty = false;
        return _data;
    }
    
    public void Dirty() => _dirty = true;
    public void Clean(TData data) => _data = data;
}