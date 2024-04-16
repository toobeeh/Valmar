using Microsoft.Extensions.Options;
using Valmar.Domain.Classes;
using Valmar.Domain.Implementation.Drops;
using Valmar.Util;
using Valmar.Util.NChunkTree;
using Valmar.Util.NChunkTree.Bubbles;

namespace Valmar.Domain.Implementation.Bubbles;

/// <summary>
/// Implementation which extends the bubblechunktree node by the chunk specific functions
/// Main feature is the caching of the results of the underlying chunks
/// </summary>
public class CachedBubbleChunk : BubbleChunkTree, IBubbleChunk
{
    private readonly CachedBubbleChunkContext _context;
    public CachedBubbleChunk(
        IServiceProvider services, 
        IOptions<BubbleChunkConfiguration> config, 
        BubbleChunkTreeProvider provider, 
        NChunkTreeNodeContext context) : base(services, config, provider, context)
    {
        _context = provider.CachedChunkContext.GetOrAdd(NodeId, new CachedBubbleChunkContext());
    }

    public override IBubbleChunk Chunk => this;
    public int? TraceIdStart => Chunks.Count == 0 ? null : Chunks.First().TraceIdStart;
    public int? TraceIdEnd => Chunks.Count == 0 ? null : Chunks.Last().TraceIdEnd;
    public DateTimeOffset? TraceTimestampStart => Chunks.Count == 0 ? null : Chunks.First().TraceTimestampStart;
    public DateTimeOffset? TraceTimestampEnd => Chunks.Count == 0 ? null : Chunks.Last().TraceTimestampEnd;
    private List<IBubbleChunk> Chunks => Nodes.Select(node => node.Chunk).ToList(); // by accessing "Nodes" it will create new instances every time - this actually makes it completely thread-safe
    
    public async Task<List<int>> EvaluateSubChunks(int chunkSize)
    {
        var subChunks = new List<int>();
        foreach (var chunk in Chunks)
        {
            subChunks.AddRange(await chunk.EvaluateSubChunks(chunkSize));
        }
        return subChunks;
    }
    
    public async Task<DateTimeOffset?> GetFirstSeenDate(int login)
    {
        // get key for request identifiers
        var key = $"{login}";
        
        var store = _context.FirstSeenDates.GetOrAdd(key,key =>  new KeyValueStore<string, DateTimeOffset?>(key, async key =>
            await ChunkHelper.ReduceParallel(Chunks, async c => await c.GetFirstSeenDate(login), (a, b) =>
            {
                if (a is null) return b;
                if (b is null) return a;
                
                return a < b ? a : b;
            }, null))
        );
        return await store.Retrieve();
    }

    public async Task<BubbleTimespanRangeDdo> GetAmountCollectedInTimespan(int login, DateTimeOffset? start, DateTimeOffset? end)
    {
        // sort out if this chunk is relevant - -1 signalizes this value should be ignored
        if (start > TraceTimestampEnd) return new BubbleTimespanRangeDdo(null, null);
        if (end < TraceTimestampStart) return new BubbleTimespanRangeDdo(null, null);
        
        // set end or start to null if out if chunk range
        if (start < TraceTimestampStart) start = null;
        if (end >= TraceTimestampEnd) end = null;
        
        // create key for request identifiers
        var key = $"{login}//{start}//{end}";
        
        // set as dirty if chunk is open ended (can always be bigger than last checked!)
        if(TraceIdEnd is null && _context.CollectedBubbles.TryGetValue(key, out var bubbleStore)) bubbleStore.Dirty();

        var store = _context.CollectedBubbles.GetOrAdd(key,key =>  new KeyValueStore<string, BubbleTimespanRangeDdo>(key, async key =>
            await ChunkHelper.ReduceParallel(Chunks, async c => await c.GetAmountCollectedInTimespan(login, start, end), (a, b) =>
            {
                // treat cases where one of the ranges has absolutely no matches
                var aNoMatches = a.StartAmount is null && a.EndAmount is null;
                var bNoMatches = b.StartAmount is null && b.EndAmount is null;
                if(aNoMatches && bNoMatches) return a;
                if(aNoMatches && !bNoMatches) return b;
                if(!aNoMatches && bNoMatches) return a;
                
                // one of range values null -> invalid state! incorrect calculation, cannot happen, should be equal in case of only one trace
                if(a.StartAmount is null || a.EndAmount is null || b.StartAmount is null || b.EndAmount is null) throw new InvalidOperationException("Invalid state: Exactly one of the trace range values is null");
                
                return new BubbleTimespanRangeDdo(a.StartAmount, b.EndAmount);
            }, new BubbleTimespanRangeDdo(null, null)))
        );
        return await store.Retrieve();
    }
}