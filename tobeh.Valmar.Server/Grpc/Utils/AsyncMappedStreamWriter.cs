using Grpc.Core;

namespace tobeh.Valmar.Server.Grpc.Utils;

public static class AsyncMappedStreamWriter
{
    public static async Task WriteAllMappedAsync<TSource, TMapped>(this IServerStreamWriter<TMapped> streamWriter,
        IEnumerable<TSource> items, Func<TSource, TMapped> mapping)
    {
        foreach (TSource item in items)
        {
            TMapped mappedItem = mapping.Invoke(item);
            await streamWriter.WriteAsync(mappedItem);
        }
    }

    public static async Task WriteAllMappedAsync<TSource, TMapped>(this IServerStreamWriter<TMapped> streamWriter,
        IEnumerable<TSource> items, Func<TSource, Task<TMapped>> mapping, bool continueOnError = false)
    {
        foreach (TSource item in items)
        {
            TMapped mappedItem;
            try
            {
                mappedItem = await mapping.Invoke(item);
            }
            catch (Exception e)
            {
                if (continueOnError)
                {
                    continue;
                }

                throw;
            }

            await streamWriter.WriteAsync(mappedItem);
        }
    }
}