using System.Collections.Concurrent;

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

    private async Task<double> SumAsync<TSource>(IEnumerable<TSource> source, Func<TSource, Task<double>> sourceMapping)
    {
        double sum = 0;
        var sumLock = new object();
        
        await Parallel.ForEachAsync(source, async (item, token) =>
        {
            try
            {
                var value = await sourceMapping.Invoke(item);
                lock (sumLock)
                {
                    sum += value;
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

    public async Task<double> GetTotalLeagueWeight(string id)
    {
        var store = _leagueDropValue.GetOrAdd(id,id =>  new UserStore<string, double>(id, async id =>
            await SumAsync(chunks, async c => await c.GetTotalLeagueWeight(id)))
        );
        return await store.Retrieve();
    }

    public async Task<double> GetLeagueWeightInTimespan(string id, DateOnly start, DateOnly end)
    {
        throw new NotImplementedException();
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