using System.Collections.Concurrent;
using Valmar.Util;
using Valmar.Util.NChunkTree;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation.Drops;

/// <summary>
/// Implementation which extends the dropchunktree node by the chunk specific functions
/// Main feature is the caching of the results of the underlying chunks
/// </summary>
public class CachedDropChunk : DropChunkTree, IDropChunk
{
    private readonly CachedDropChunkContext _context;
    public CachedDropChunk(IServiceProvider services, DropChunkTreeProvider provider, NChunkTreeNodeContext context) : base(services, provider, context)
    {
        _context = provider.CachedChunkContext.GetOrAdd(NodeId, new CachedDropChunkContext());
    }

    public override IDropChunk Chunk => this;
    public long? DropIndexStart => Chunks.Count == 0 ? null : Chunks.First().DropIndexStart;
    public long? DropIndexEnd => Chunks.Count == 0 ? null : Chunks.Last().DropIndexEnd;
    public DateTimeOffset? DropTimestampStart => Chunks.Count == 0 ? null : Chunks.First().DropTimestampStart;
    public DateTimeOffset? DropTimestampEnd => Chunks.Count == 0 ? null : Chunks.Last().DropTimestampEnd;
    private List<IDropChunk> Chunks => Nodes.Select(node => node.Chunk).ToList(); // by accessing "Nodes" it will create new isntances every time - this actually makes it completely thread-safe+
    

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
        if(DropIndexEnd is null && _context.LeagueDropValue.ContainsKey(key)) _context.LeagueDropValue[key].Dirty();

        var store = _context.LeagueDropValue.GetOrAdd(key,key =>  new UserStore<string, double>(key, async key =>
            await DropHelper.ReduceParallel(Chunks, async c => await c.GetLeagueWeight(id, start, end), (a, b) => a+b, 0d))
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
        if(DropIndexEnd is null && _context.LeagueParticipants.ContainsKey(key)) _context.LeagueParticipants[key].Dirty();

        var store = _context.LeagueParticipants.GetOrAdd(key,key =>  new UserStore<string, IList<string>>(key, async key =>
            await DropHelper.ReduceParallel(Chunks, async c => await c.GetLeagueParticipants(start, end), (a, b) =>
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
        if(DropIndexEnd is null && _context.LeagueDropCount.ContainsKey(key)) _context.LeagueDropCount[key].Dirty();

        var store = _context.LeagueDropCount.GetOrAdd(key,key =>  new UserStore<string, int>(key, async key =>
            await DropHelper.ReduceParallel(Chunks, async c => await c.GetLeagueCount(id, start, end), (a, b) => a+b, 0))
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
        if(DropIndexEnd is null && _context.LeagueStreak.ContainsKey(key)) _context.LeagueStreak[key].Dirty();
        
        var store = _context.LeagueStreak.GetOrAdd(key,key =>  new UserStore<string, StreakResult>(key, async key =>
            await DropHelper.ReduceParallel(Chunks, async c => await c.GetLeagueStreak(id, start, end), (a, b) =>
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
        if(DropIndexEnd is null && _context.LeagueAverageTime.ContainsKey(key)) _context.LeagueAverageTime[key].Dirty();
            
        double amount = await GetLeagueCount(id, start, end);
        var store = _context.LeagueAverageTime.GetOrAdd(key,key => new UserStore<string, double>(key, async key =>
            amount == 0 ? 0 : await DropHelper.ReduceParallel(Chunks, async c =>
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
        if(DropIndexEnd is null && _context.EventDetails.ContainsKey(key)) _context.EventDetails[key].Dirty();

        var store = _context.EventDetails.GetOrAdd(key,key =>  new UserStore<string, EventResult>(key, async key =>
            await DropHelper.ReduceParallel(Chunks, async c => await c.GetEventLeagueDetails(eventId, userid, userLogin),
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