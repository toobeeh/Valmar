using System.Collections.Concurrent;
using Microsoft.AspNetCore.Identity;

namespace Valmar.Util.NChunkTree;

/*
 *  Drop Store should accomplish the following:
 *  - Get user total drop value (regular and event drops)
 *  - Get user league score/streak/count in time span
 *  - get user event drop value (used/unused)
 *  - consume user eventdrop value
 *
 */
public class CachedDropChunk(IEnumerable<IDropChunk> chunks, long? dropIndexStart, long? dropIndexEnd) : IDropChunk
{
    private ConcurrentDictionary<string, UserStore<string, double>> _leagueDropValue = new ();

    private async Task<double> SumAsync<TSource>(IEnumerable<TSource> source, Func<TSource, Task<double>> sourceMapping)
    {
        double sum = 0;
        var sumLock = new object();
        
        await Parallel.ForEachAsync(source, async (item, token) =>
        {
            var value = await sourceMapping.Invoke(item);
            lock (sumLock)
            {
                sum += value;
            }
        });

        return sum;
    }
    public long? DropIndexStart => dropIndexStart;
    public long? DropIndexEnd => dropIndexEnd;
    
    public async Task<double> GetTotalDropValueForUser(string id)
    {
        var store = _leagueDropValue.GetOrAdd(id,id =>  new UserStore<string, double>(id, async id =>
            await SumAsync(chunks, async c => await c.GetTotalDropValueForUser(id)))
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