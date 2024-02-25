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
 * IMPROVEMENTS
 * -> thread-unsafe and not nice with DI as-is
 *
 * add a service which is a singleton and contains data that is shared across the tree
 * service creates new tree instance with id
 * rather than assigning chunk instances, chunk IDs are assigned and fetched from the nodes
 * provider service stores the state of the tree
 *
 * - tree base class <TChunk , TProvider>
 * - provider base class
 *
 * -> drop tree/leaf class implements TChunk-specific behavior
 * -> drop provider class
 *
 * goal: state should be singleton; services and tree transient or scoped for multithreading.
 * state (cache service) needs to be designed thread-safe
 *
 * ... make league-fetching methods all in one instead per-user
 * results in lesser db queries & overhead which improves advantage of chunking since chunk-data/cpu-intensity becomes more dense
 *
 */
public class CachedDropChunk(IEnumerable<IDropChunk> chunks) : IDropChunk
{
    private readonly ConcurrentDictionary<string, UserStore<string, double>> _leagueDropValue = new ();
    private readonly ConcurrentDictionary<string, UserStore<string, int>> _leagueDropCount = new ();
    private readonly ConcurrentDictionary<string, UserStore<string, StreakResult>> _leagueStreak = new ();
    private readonly ConcurrentDictionary<string, UserStore<string, double>> _leagueAverageTime = new ();
    private readonly ConcurrentDictionary<string, UserStore<string, EventResult>> _eventDetails = new ();
    private readonly ConcurrentDictionary<string, UserStore<string, IList<string>>> _leagueParticipants = new ();
    
    private async Task<TSum> ReduceParallel<TSource, TSum>(IEnumerable<TSource> source, Func<TSource, Task<TSum>> sourceMapping, Func<TSum, TSum, TSum> aggregator, TSum seed)
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
    public long? DropIndexEnd => !chunks.Any() ? null : chunks.Last().DropIndexEnd;
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
        
        // set as dirty if chunk is open ended (can always be bigger than last checked!)
        if(DropIndexEnd is null && _leagueDropValue.ContainsKey(key)) _leagueDropValue[key].Dirty();

        var store = _leagueDropValue.GetOrAdd(key,key =>  new UserStore<string, double>(key, async key =>
            await ReduceParallel(chunks, async c => await c.GetLeagueWeight(id, start, end), (a, b) => a+b, 0d))
        );
        return await store.Retrieve();
    }
    
    public async Task<IList<string>> GetLeagueParticipants()
    {
        return await GetLeagueParticipants(null, null);
    }

    public async Task<IList<string>> GetLeagueParticipants(DateTimeOffset? start, DateTimeOffset? end)
    {
        // sort out if this chunk is relevant
        if (start > DropTimestampEnd) return [];
        if (end < DropTimestampStart) return [];
        
        // set end or start to null if out if chunk range
        if (start < DropTimestampStart) start = null;
        if (end >= DropTimestampEnd) end = null;
        
        // use for consistency same store-retrieval method, although ever request shares the same resource
        var key = $"all-have-the-same-key";
        
        // set as dirty if chunk is open ended (can always be bigger than last checked!)
        if(DropIndexEnd is null && _leagueParticipants.ContainsKey(key)) _leagueParticipants[key].Dirty();

        var store = _leagueParticipants.GetOrAdd(key,key =>  new UserStore<string, IList<string>>(key, async key =>
            await ReduceParallel(chunks, async c => await c.GetLeagueParticipants(start, end), (a, b) =>
            {
                foreach (var item in b)
                {
                    if (!a.Contains(item)) a.Add(item);
                }
                return a;
            }, []))
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
        
        // set as dirty if chunk is open ended (can always be bigger than last checked!)
        if(DropIndexEnd is null && _leagueDropCount.ContainsKey(key)) _leagueDropCount[key].Dirty();

        var store = _leagueDropCount.GetOrAdd(key,key =>  new UserStore<string, int>(key, async key =>
            await ReduceParallel(chunks, async c => await c.GetLeagueCount(id, start, end), (a, b) => a+b, 0))
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
        
        // set as dirty if chunk is open ended (can always be bigger than last checked!)
        if(DropIndexEnd is null && _leagueStreak.ContainsKey(key)) _leagueStreak[key].Dirty();
        
        var store = _leagueStreak.GetOrAdd(key,key =>  new UserStore<string, StreakResult>(key, async key =>
            await ReduceParallel(chunks, async c => await c.GetLeagueStreak(id, start, end), (a, b) =>
            {
                var intersection = a.Head + b.Tail;
                var max = Math.Max(b.Streak, Math.Max(a.Streak, intersection));
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
        
        // set as dirty if chunk is open ended (can always be bigger than last checked!)
        if(DropIndexEnd is null && _leagueAverageTime.ContainsKey(key)) _leagueAverageTime[key].Dirty();
            
        double amount = await GetLeagueCount(id, start, end);
        var store = _leagueAverageTime.GetOrAdd(key,key => new UserStore<string, double>(key, async key =>
            amount == 0 ? 0 : await ReduceParallel(chunks, async c =>
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
        
        // set as dirty if chunk is open ended (can always be bigger than last checked!)
        if(DropIndexEnd is null && _eventDetails.ContainsKey(key)) _eventDetails[key].Dirty();

        var store = _eventDetails.GetOrAdd(key,key =>  new UserStore<string, EventResult>(key, async key =>
            await ReduceParallel(chunks, async c => await c.GetEventLeagueDetails(eventId, userid, userLogin),
                (a, b) =>
                {
                    foreach (var key in b.RedeemableCredit.Keys)
                    {
                        // add all redeemable drop Ids to dictionary
                        a.RedeemableCredit.AddOrUpdate(key, b.RedeemableCredit[key], (akey, value) =>
                        {
                            foreach (var dropId in b.RedeemableCredit[key].Keys)
                            {
                                value.TryAdd(dropId, b.RedeemableCredit[key][dropId]);
                            }

                            return value;
                        });
                    }
                    
                    return a with { Progress = a.Progress + b.Progress };
                }, 
                new EventResult(new ConcurrentDictionary<int, ConcurrentDictionary<long, double>>(), 0)))
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