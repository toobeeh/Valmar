using Valmar.Database;

namespace Valmar.Util.NChunkTree;

public class DropChunkLeaf : NChunkTree<IDropChunkNode>, IDropChunkNode
{
    public static List<PastDropEntity> drops;
    protected override long? ChunkStartIndex => DropChunk.DropIndexStart;
    protected override long? ChunkEndIndex => DropChunk.DropIndexEnd;
    protected override IDropChunkNode Chunk => this;
    
    public DropChunkLeaf(long dropIndexStart, long dropIndexEnd) : base(0)
    {
        DropChunk = new PersistentDropChunk(dropIndexStart, dropIndexEnd);
    }

    public IDropChunk DropChunk { get; }
    protected override NChunkTree<IDropChunkNode> CreateExpansionNode()
    {
        return new DropChunkTree(ChunkNumber);
    }
}