using Microsoft.Extensions.Options;
using Valmar.Domain.Implementation.Bubbles;
using Valmar.Domain.Implementation.Drops;
using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Util.NChunkTree.Bubbles;

/// <summary>
/// Implementation of the nchunktree for bubbles
/// designed to be a node in the tree
/// 
/// implements functions specific to the tree
/// for implementation of the chunk, see the cachedbubblechunk class
/// </summary>
/// <param name="services"></param>
/// <param name="provider"></param>
/// <param name="context"></param>
public abstract class BubbleChunkTree(
    IServiceProvider services,
    IOptions<BubbleChunkConfiguration> config,
    BubbleChunkTreeProvider provider,
    NChunkTreeNodeContext context)
    : NChunkTree<IBubbleChunk, BubbleChunkTreeProvider>(services, provider, context)
{
    private readonly BubbleChunkTreeProvider _provider = provider;
    protected override long? ChunkStartIndex => Chunk.TraceIdStart;
    protected override long? ChunkEndIndex => Chunk.TraceIdEnd;
    protected override NChunkTree<IBubbleChunk, BubbleChunkTreeProvider> CreateExpansionNode()
    {
        return Provider.CreateNode<IBubbleChunk, BubbleChunkTreeProvider, CachedBubbleChunk>(NodeCount, Level);
    }

    public override NChunkTree<IBubbleChunk, BubbleChunkTreeProvider> AddChunk(NChunkTree<IBubbleChunk, BubbleChunkTreeProvider> chunk)
    {
        Provider.CachedChunkContext.Remove(NodeId, out _); // remove everything cached due to chunk resize
        return base.AddChunk(chunk);
    }

    public override void RepartitionChunks()
    {
        // only the last chunk may be repartitioned because repartitioning others could destroy integrity of chunk tree - and simply not possible for drops
        var candidate = Nodes.Last();
        
        // check if node is open-ended; if not, it does not need to be repartitioned since its amount is assumed to be correct (unchanged)
        if(candidate.Chunk.TraceIdEnd is not null)
        {
            return;
        }
        
        // check if node is a leaf - if yes, perform operation on it. else, delegate repartitioning to the node
        if (Provider.GetNodeCardinality(candidate.NodeId) > 1)
        {
            candidate.RepartitionChunks();
            return;
        }

        // evaluate if new subchunks can be made in chunk
        var subChunks = candidate.Chunk.EvaluateSubChunks(config.Value.ChunkSize).Result;
        
        // first index should equal or bigger than chunk start index to maintain integrity
        if(subChunks.Count == 0 || subChunks[0] < candidate.Chunk.TraceIdStart)
        {
            throw new InvalidOperationException("Error calculating subchunks - First subchunk should start at the same index as the chunk");
        }
        
        // if only one chunk as result, no need for repartitioning
        if(subChunks.Count == 1)
        {
            return;
        }
        
        // remove the last chunk and add the new subchunks
        Provider.RemoveNode(candidate.NodeId, NodeId);
        
        // add new chunks
        for(int i = 0; i < subChunks.Count; i++)
        {
            var newChunk = Provider.CreateLeaf(subChunks[i], i + 1 > subChunks.Count - 1 ? null : subChunks[i + 1]);
            _provider.AddLeaf(newChunk);
        }
    }
}