using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using Valmar.Util;
using Valmar.Util.NChunkTree;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Domain.Implementation.Drops;

/// <summary>
/// Implementation which extends the dropchunktree node by the chunk specific functions
/// Main feature is the caching of the results of the underlying chunks
///
/// refactored goals:
/// - drop value caching per-user per-timespan
/// - event stats caching per-user
/// - league stats caching per-time
/// - server leaderboard caching per-server
///
/// </summary>
public class CachedDropChunk : DropChunkTree, IDropChunk
{
    private readonly CachedDropChunkContext _context;
    public CachedDropChunk(
        IServiceProvider services, 
        IOptions<DropChunkConfiguration> config, 
        DropChunkTreeProvider provider, 
        NChunkTreeNodeContext context) : base(services, config, provider, context)
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

        var store = _context.LeagueDropValue.GetOrAdd(key,key =>  new KVStore<string, double>(key, async key =>
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
        var key = $"{start}//{end}";
        
        // set as dirty if chunk is open ended (can always be bigger than last checked!)
        if(DropIndexEnd is null && _context.LeagueParticipants.ContainsKey(key)) _context.LeagueParticipants[key].Dirty();

        var store = _context.LeagueParticipants.GetOrAdd(key,key =>  new KVStore<string, IList<string>>(key, async key =>
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

        var store = _context.LeagueDropCount.GetOrAdd(key,key =>  new KVStore<string, int>(key, async key =>
            await DropHelper.ReduceParallel(Chunks, async c => await c.GetLeagueCount(id, start, end), (a, b) => a+b, 0))
        );
        return await store.Retrieve();
    }
    
    public async Task<List<long>> EvaluateSubChunks(int chunkSize)
    {
        var subChunks = new List<long>();
        foreach (var chunk in Chunks)
        {
            subChunks.AddRange(await chunk.EvaluateSubChunks(chunkSize));
        }
        return subChunks;
    }
    
    public async Task<EventResult> GetEventLeagueDetails(int eventId, string userid, int userLogin)
    {
        // get key for request identifiers
        var key = $"{eventId}//{userid}//{userLogin}";
        
        // set as dirty if chunk is open ended (can always be bigger than last checked!)
        if(DropIndexEnd is null && _context.EventDetails.ContainsKey(key)) _context.EventDetails[key].Dirty();

        var store = _context.EventDetails.GetOrAdd(key,key =>  new KVStore<string, EventResult>(key, async key =>
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

    public async Task<Dictionary<string, LeagueResult>> GetLeagueResults(DateTimeOffset? start, DateTimeOffset? end)
    {
        // get key for request identifiers
        var key = $"{start}//{end}";
        
        // set as dirty if chunk is open ended (can always be bigger than last checked!)
        if(DropIndexEnd is null && _context.LeagueResults.TryGetValue(key, out var detail)) detail.Dirty();
        
        
        var store = _context.LeagueResults.GetOrAdd(key,_ =>  new KVStore<string, Dictionary<string, LeagueResult>>(key, async _ =>
            await DropHelper.ReduceParallel(Chunks, async c => await c.GetLeagueResults(start, end),
                (a, b) =>
                {
                    foreach (var result in b.Values)
                    {
                        // check if user is present in other chunk
                        if (a.TryGetValue(result.Id, out var aResult))
                        {
                            var totalCount = result.Count + aResult.Count;
                            var totalScore = result.Score + aResult.Score;
                            var combinedAverageTime = result.AverageTime * result.Count / totalCount +
                                                      aResult.AverageTime * result.Count / totalCount;
                            
                            var combinedAverageWeight = result.AverageWeight * result.Count / totalCount +
                                                      aResult.AverageWeight * result.Count / totalCount;

                            var combinedStreak = new StreakResult(
                                result.Streak.Tail,
                                aResult.Streak.Head,
                                Math.Max(Math.Max(result.Streak.Streak, aResult.Streak.Streak),
                                    aResult.Streak.Head + result.Streak.Tail));
                                
                            var combinedResult = new LeagueResult(
                                result.Id,
                                totalScore,
                                totalCount,
                                combinedAverageTime,
                                combinedAverageWeight,
                                combinedStreak);

                            a[result.Id] = combinedResult;
                        }
                        
                        // if not, just add result of chunk
                        else
                        {
                            a[result.Id] = result;
                        }
                    }

                    return a;
                }, 
                new Dictionary<string, LeagueResult>()))
        );
        var val = await store.Retrieve();
        
        return val;
    }

}