using Grpc.Core;

namespace Valmar.Grpc.Utils;

public static class AsyncMappedStreamWriter
{
    public static async Task WriteAllMappedAsync<TSource, TMapped>(this IServerStreamWriter<TMapped> streamWriter, IEnumerable<TSource> items, Func<TSource, TMapped> mapping){
        foreach (TSource item in items)
        {
            TMapped mappedItem = mapping.Invoke(item);
            await streamWriter.WriteAsync(mappedItem);
        }
    }
    
    public static async Task WriteAllMappedAsync<TSource, TMapped>(this IServerStreamWriter<TMapped> streamWriter, IEnumerable<TSource> items, Func<TSource, Task<TMapped>> mapping){
        foreach (TSource item in items)
        {
            TMapped mappedItem = await mapping.Invoke(item);
            await streamWriter.WriteAsync(mappedItem);
        }
    }
}