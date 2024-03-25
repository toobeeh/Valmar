using Microsoft.Extensions.Options;
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

    public DateTimeOffset GetFirstSeenDate(int login)
    {
        throw new NotImplementedException();
    }
}