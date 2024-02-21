namespace Valmar.Util.NChunkTree;

public class DropChunkTree : NChunkTree<IDropChunkNode>, IDropChunkNode
{
    protected override long? ChunkStartIndex {
        get
        {
            var chunks = Chunks.ToList();
            if (chunks.Count == 0) return null;
            else return chunks.First().DropChunk.DropIndexStart;
        }
    }
    protected override long? ChunkEndIndex  {
        get
        {
            var chunks = Chunks.ToList();
            if (chunks.Count == 0) return null;
            else return chunks.Last().DropChunk.DropIndexStart;
        }
    }
    protected override IDropChunkNode Chunk => this;
    protected override NChunkTree<IDropChunkNode> CreateExpansionNode()
    {
        return new DropChunkTree(ChunkNumber);
    }
    
    public DropChunkTree(int chunkCount) : base(chunkCount)
    {
        DropChunk = InitChunk();
    }

    private CachedDropChunk InitChunk()
    {
        return new CachedDropChunk(Chunks.Select(c => c.DropChunk), ChunkStartIndex, ChunkEndIndex); // effectively dirty all values (remove all) from this node
    }

    public override NChunkTree<IDropChunkNode> AddChunk(NChunkTree<IDropChunkNode> chunk)
    {
        DropChunk = InitChunk();
        return base.AddChunk(chunk);
    }

    public IDropChunk DropChunk { get; private set; }
}