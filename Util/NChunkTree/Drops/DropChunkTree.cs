using Valmar.Domain.Implementation.Drops;

namespace Valmar.Util.NChunkTree.Drops;

public class DropChunkTree : NChunkTree<IDropChunk>
{
    private readonly IServiceProvider _services;
    private CachedDropChunk _chunk;
    protected override long? ChunkStartIndex => Chunk.DropIndexStart;
    protected override long? ChunkEndIndex => Chunk.DropIndexEnd;
    public sealed override IDropChunk Chunk => _chunk;
    protected override NChunkTree<IDropChunk> CreateExpansionNode()
    {
        return new DropChunkTree(_services, ChunkNumber);
    }
    
    public DropChunkTree(IServiceProvider services, int chunkCount) : base(chunkCount)
    {
        _services = services;
        _chunk = InitChunk();
    }

    private CachedDropChunk InitChunk()
    {
        return new CachedDropChunk(Chunks); // effectively dirty all values (remove all) from this node
    }

    public override NChunkTree<IDropChunk> AddChunk(NChunkTree<IDropChunk> chunk)
    {
        _chunk = InitChunk();
        return base.AddChunk(chunk);
    }
}