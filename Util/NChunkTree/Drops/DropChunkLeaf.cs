using Valmar.Domain.Implementation.Drops;

namespace Valmar.Util.NChunkTree.Drops;

public class DropChunkLeaf : NChunkTree<IDropChunk>
{
    
    private readonly IServiceProvider _services;
    private readonly PersistentDropChunk _chunk;
    protected override long? ChunkStartIndex => Chunk.DropIndexStart;
    protected override long? ChunkEndIndex => Chunk.DropIndexEnd;
    public sealed override IDropChunk Chunk => _chunk;
    
    public DropChunkLeaf(IServiceProvider services) : base(0)
    {
        _chunk = ActivatorUtilities.CreateInstance<PersistentDropChunk>(services);
        _services = services;
    }
    public DropChunkLeaf WithChunkSize(long? dropIndexStart, long? dropIndexEnd)
    {
        _chunk.SetChunkSize(dropIndexStart, dropIndexEnd);
        return this;
    } 
    protected override NChunkTree<IDropChunk> CreateExpansionNode()
    {
        throw new InvalidOperationException("Leaves cannot expand");
    }
}