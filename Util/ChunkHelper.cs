namespace Valmar.Util;

public static class ChunkHelper
{
    public static async Task<TSum> ReduceParallel<TSource, TSum>(IEnumerable<TSource> source, Func<TSource, Task<TSum>> sourceMapping, Func<TSum, TSum, TSum> aggregator, TSum seed)
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
}