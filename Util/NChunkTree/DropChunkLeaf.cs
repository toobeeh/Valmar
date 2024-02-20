using Valmar.Database;

namespace Valmar.Util.NChunkTree;

public class DropChunkLeaf : NChunkTree<IDropChunkNode>, IDropChunkNode
{
    private static List<PastDropEntity> drops;
    
    public DropChunkLeaf(long dropIndexStart, long dropIndexEnd) : base(0)
    {
        DropIndexStart = dropIndexStart;
        DropIndexEnd = dropIndexEnd;
    }
    protected override long ChunkStartIndex => DropIndexStart;
    protected override long ChunkEndIndex => DropIndexEnd;
    protected override NChunkTree<IDropChunkNode> CreateExpansionNode()
    {
        return new DropChunkTree(ChunkNumber);
    }

    protected override IDropChunkNode Chunk => this;
    
    public long DropIndexStart { get; }

    public long DropIndexEnd { get; }
}