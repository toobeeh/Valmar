using Valmar.Domain.Implementation.Drops;

namespace Valmar.Util.NChunkTree.Drops;

/// <summary>
/// Implementation of the nchunktree for drops
/// designed to be a node in the tree
/// 
/// implements functions specific to the tree
/// for implementation of the chunk, see the cacheddropchunk class
/// </summary>
/// <param name="services"></param>
/// <param name="provider"></param>
/// <param name="context"></param>
public abstract class DropChunkTree(
    IServiceProvider services,
    DropChunkTreeProvider provider,
    NChunkTreeNodeContext context)
    : NChunkTree<IDropChunk, DropChunkTreeProvider>(services, provider, context)
{
    protected override long? ChunkStartIndex => Chunk.DropIndexStart;
    protected override long? ChunkEndIndex => Chunk.DropIndexEnd;
    protected override NChunkTree<IDropChunk, DropChunkTreeProvider> CreateExpansionNode()
    {
        return Provider.CreateNode<IDropChunk, DropChunkTreeProvider, CachedDropChunk>(services, NodeCount, Level);
    }

    public override NChunkTree<IDropChunk, DropChunkTreeProvider> AddChunk(NChunkTree<IDropChunk, DropChunkTreeProvider> chunk)
    {
        // TODO dirty chunk
        return base.AddChunk(chunk);
    }
}