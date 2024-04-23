using tobeh.Valmar.Server.Domain.Implementation.Drops;

namespace tobeh.Valmar.Server.Util.NChunkTree.Drops;

/// <summary>
/// Implementation of the nchunktree for drops
/// designed to be a leaf in the tree
/// 
/// implements functions specific to the tree
/// for implementation of the chunk, see the persistentdropchunk class
/// </summary>
/// <param name="services"></param>
/// <param name="provider"></param>
/// <param name="context"></param>
public abstract class DropChunkLeaf(
    IServiceProvider services,
    DropChunkTreeProvider provider,
    NChunkTreeNodeContext context)
    : NChunkTree<IDropChunk, DropChunkTreeProvider>(services, provider, context)
{
    protected override long? ChunkStartIndex => Chunk.DropIndexStart;
    protected override long? ChunkEndIndex => Chunk.DropIndexEnd;
    protected override NChunkTree<IDropChunk, DropChunkTreeProvider> CreateExpansionNode()
    {
        throw new InvalidOperationException("Leaves cannot expand");
    }

    public override void RepartitionChunks()
    {
        throw new InvalidOperationException("Node is a leaf and has no chunks; repartitioning can only be done in a parent node");
    }
}