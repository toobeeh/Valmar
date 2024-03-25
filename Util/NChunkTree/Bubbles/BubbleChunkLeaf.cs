using Valmar.Util.NChunkTree.Drops;

namespace Valmar.Util.NChunkTree.Bubbles;

/// <summary>
/// Implementation of the nchunktree for bubbles
/// designed to be a leaf in the tree
/// 
/// implements functions specific to the tree
/// for implementation of the chunk, see the persistentbubblechunk class
/// </summary>
/// <param name="services"></param>
/// <param name="provider"></param>
/// <param name="context"></param>
public abstract class BubbleChunkLeaf(
    IServiceProvider services,
    BubbleChunkTreeProvider provider,
    NChunkTreeNodeContext context)
    : NChunkTree<IBubbleChunk, BubbleChunkTreeProvider>(services, provider, context)
{
    protected override long? ChunkStartIndex => Chunk.TraceIdStart;
    protected override long? ChunkEndIndex => Chunk.TraceIdEnd;
    protected override NChunkTree<IBubbleChunk, BubbleChunkTreeProvider> CreateExpansionNode()
    {
        throw new InvalidOperationException("Leaves cannot expand");
    }

    public override void RepartitionChunks()
    {
        throw new InvalidOperationException("Node is a leaf and has no chunks; repartitioning can only be done in a parent node");
    }
}