using Valmar.Domain.Implementation.Drops;

namespace Valmar.Util.NChunkTree.Drops;

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
    protected override bool IsFull => true;
    protected override int Cardinality => 1;
    protected override int NodeCount => 0;
    protected override List<NChunkTree<IDropChunk, DropChunkTreeProvider>> Nodes => new();
}