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
    private ConcurrentDictionary<string, UserStore<string, double>> _leagueTimespanDropValue = new ();
    private ConcurrentDictionary<string, UserStore<string, int>> _leagueTimespanDropCount = new ();
    
    private async Task<TSum> SumAsync<TSource, TSum>(IEnumerable<TSource> source, Func<TSource, Task<TSum>> sourceMapping, Func<TSum, TSum, TSum> aggregator)
    {
        TSum sum = default;
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

    public async Task<double> GetTotalLeagueWeight(string id)
    {
        var store = _leagueDropValue.GetOrAdd(id,id =>  new UserStore<string, double>(id, async id =>
            await SumAsync(chunks, async c => await c.GetTotalLeagueWeight(id), (a, b) => a+b))
        );
        return await store.Retrieve();
    }

    public async Task<double> GetLeagueWeightInTimespan(string id, DateTimeOffset start, DateTimeOffset end)
    {
        // sort out if this chunk is relevant
        if (start > DropTimestampEnd) return 0d;
        if (end < DropTimestampStart) return 0d;
        
        // return whole chunk if it's within the timespan
        if (start <= DropTimestampStart && end >= DropTimestampEnd)
        {
            return await GetTotalLeagueWeight(id);
        }
        
        // get key for request identifiers; will mostly be league requests so not too diverse
        var key = $"{id}//{start.UtcTicks}//{end.UtcTicks}";
        var store = _leagueTimespanDropValue.GetOrAdd(key,key =>  new UserStore<string, double>(key, async key =>
            await SumAsync(chunks, async c => await c.GetLeagueWeightInTimespan(id, start, end), (a, b) => a+b))
        );
        return await store.Retrieve();
    }
    
    public async Task<int> GetTotalLeagueCount(string id)
    {
        var store = _leagueDropCount.GetOrAdd(id,id =>  new UserStore<string, int>(id, async id =>
            await SumAsync(chunks, async c => await c.GetTotalLeagueCount(id), (a, b) => a+b))
        );
        return await store.Retrieve();
    }

    public async Task<int> GetLeagueCountInTimespan(string id, DateTimeOffset start, DateTimeOffset end)
    {
        // sort out if this chunk is relevant
        if (start > DropTimestampEnd) return 0;
        if (end < DropTimestampStart) return 0;
        
        // return whole chunk if it's within the timespan
        if (start <= DropTimestampStart && end >= DropTimestampEnd)
        {
            return await GetTotalLeagueCount(id);
        }
        
        // get key for request identifiers; will mostly be league requests so not too diverse
        var key = $"{id}//{start.UtcTicks}//{end.UtcTicks}";
        var store = _leagueTimespanDropCount.GetOrAdd(key,key =>  new UserStore<string, int>(key, async key =>
            await SumAsync(chunks, async c => await c.GetLeagueCountInTimespan(id, start, end), (a, b) => a+b))
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