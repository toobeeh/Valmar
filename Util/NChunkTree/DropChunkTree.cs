namespace Valmar.Util.NChunkTree;

public class DropChunkTree : NChunkTree<IDropChunkNode>, IDropChunkNode
{
    public DropChunkTree(int chunkCount) : base(chunkCount) { }
    protected override long ChunkStartIndex => DropIndexStart;
    protected override long ChunkEndIndex => DropIndexEnd;
    protected override NChunkTree<IDropChunkNode> CreateExpansionNode()
    {
        return new DropChunkTree(ChunkNumber);
    }

    protected override IDropChunkNode Chunk => this;

    public long DropIndexStart => Chunks.First().DropIndexStart;
    public long DropIndexEnd => Chunks.Last().DropIndexEnd;
}